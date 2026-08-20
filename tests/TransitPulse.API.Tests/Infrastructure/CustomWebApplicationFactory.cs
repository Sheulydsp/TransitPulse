using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using TransitPulse.Infrastructure.Persistence;

namespace TransitPulse.API.Tests.Infrastructure;

public class CustomWebApplicationFactory
    : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgresContainer =
        new PostgreSqlBuilder("postgres:16")
            .WithDatabase("transitpulse_test")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

    public async Task InitializeAsync()
    {
        await _postgresContainer.StartAsync();

        using var scope = Services.CreateScope();

        var roleManager =
            scope.ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

        var roles = new[] { "Admin", "User" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var result = await roleManager.CreateAsync(
                    new IdentityRole(role));

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Failed to create test role '{role}'.");
                }
            }
        }
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _postgresContainer.DisposeAsync();
    }

    protected override void ConfigureWebHost(
    IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.UseSetting(
            "Logging:LogLevel:Default",
            "Debug");

        builder.UseSetting(
            "Logging:LogLevel:Microsoft.AspNetCore",
            "Debug");

        builder.UseSetting(
    "Jwt:Issuer",
    "TransitPulse");

        builder.UseSetting(
            "Jwt:Audience",
            "TransitPulseUsers");

        builder.UseSetting(
            "Jwt:SecretKey",
            "TEST_ONLY_SECRET_KEY_FOR_TRANSITPULSE_INTEGRATION_TESTS_12345678901234567890");

        builder.UseSetting(
            "Jwt:ExpiryMinutes",
            "60");

        builder.ConfigureServices(services =>
        {
            var descriptor =
                services.SingleOrDefault(
                    service => service.ServiceType ==
                        typeof(DbContextOptions<TransitPulseDbContext>));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<TransitPulseDbContext>(options =>
            {
                options.UseNpgsql(
                    _postgresContainer.GetConnectionString());
            });
        });
    }
}