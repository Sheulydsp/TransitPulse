using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TransitPulse.Application.Features.Reliability.GenerateReliabilitySnapshot;
using TransitPulse.Application.Interfaces;
using TransitPulse.Application.Services;
using TransitPulse.Infrastructure.Persistence;


namespace TransitPulse.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TransitPulseDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("TransitPulseDb")));

        services.AddScoped<IRouteEventRepository, RouteEventRepository>();

        services.AddScoped<IReliabilitySnapshotRepository, ReliabilitySnapshotRepository>();

        services.AddSingleton<IReliabilityCalculator, ReliabilityCalculator>();

        services.AddScoped<DbSeeder>();

        return services;
    }
}