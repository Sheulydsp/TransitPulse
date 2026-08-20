using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TransitPulse.Application.Features.Reliability.GenerateReliabilitySnapshot;
using TransitPulse.Application.Interfaces;
using TransitPulse.Application.Services;
using TransitPulse.Infrastructure.Authentication;
using TransitPulse.Infrastructure.Identity;
using TransitPulse.Infrastructure.Persistence;

namespace TransitPulse.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // ---------------------------------------------------------
        // Database
        // ---------------------------------------------------------

        services.AddDbContext<TransitPulseDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("TransitPulseDb")));


        // ---------------------------------------------------------
        // JWT Settings
        // ---------------------------------------------------------

        services.Configure<JwtSettings>(
            configuration.GetSection(
                JwtSettings.SectionName));


        // ---------------------------------------------------------
        // ASP.NET Core Identity
        // ---------------------------------------------------------

        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            // Password settings
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;

            // User settings
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<TransitPulseDbContext>()
        .AddDefaultTokenProviders();


        // ---------------------------------------------------------
        // JWT Authentication
        // ---------------------------------------------------------

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme =
                JwtBearerDefaults.AuthenticationScheme;

            options.DefaultChallengeScheme =
                JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var jwtSettings = configuration
                .GetSection(JwtSettings.SectionName)
                .Get<JwtSettings>()
                ?? throw new InvalidOperationException(
                    "JWT settings are not configured.");

            options.TokenValidationParameters =
                new TokenValidationParameters
                {
                    // Token validation
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    // Issuer and audience
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,

                    // Signing key
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(
                                jwtSettings.SecretKey)),

                    // Do not allow expired tokens
                    ClockSkew = TimeSpan.Zero,

                    // Tell ASP.NET Core which claim represents roles
                    RoleClaimType = ClaimTypes.Role,

                    // Tell ASP.NET Core which claim represents the user
                    NameClaimType = ClaimTypes.NameIdentifier
                };
        });


        // ---------------------------------------------------------
        // Authorization
        // ---------------------------------------------------------

        services.AddAuthorization(options =>
        {
            options.AddPolicy(
                "RequireAdmin",
                policy =>
                    policy.RequireRole("Admin"));
        });


        // ---------------------------------------------------------
        // Repositories
        // ---------------------------------------------------------

        services.AddScoped<
            IRouteEventRepository,
            RouteEventRepository>();

        services.AddScoped<
            IReliabilitySnapshotRepository,
            ReliabilitySnapshotRepository>();


        // ---------------------------------------------------------
        // Authentication Services
        // ---------------------------------------------------------

        services.AddScoped<
            IJwtTokenGenerator,
            JwtTokenGenerator>();

        services.AddScoped<
            IIdentityService,
            IdentityService>();


        // ---------------------------------------------------------
        // Application / Domain Services
        // ---------------------------------------------------------

        services.AddSingleton<
            IReliabilityCalculator,
            ReliabilityCalculator>();


        // ---------------------------------------------------------
        // Database Seeder
        // ---------------------------------------------------------

        services.AddScoped<DbSeeder>();


        return services;
    }
}