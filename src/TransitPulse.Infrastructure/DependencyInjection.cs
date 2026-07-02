using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TransitPulse.Application.Features.Reliability.GenerateReliabilitySnapshot;
using TransitPulse.Application.Interfaces;
using TransitPulse.Infrastructure.Persistence;
using TransitPulse.Infrastructure.Repositories;
using TransitPulse.Infrastructure.Services;


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

        return services;
    }
}