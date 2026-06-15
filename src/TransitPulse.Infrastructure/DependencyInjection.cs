using Microsoft.Extensions.DependencyInjection;
using TransitPulse.Application.Features.Reliability.GenerateReliabilitySnapshot;
using TransitPulse.Application.Interfaces;
using TransitPulse.Infrastructure.Repositories;
using TransitPulse.Infrastructure.Services;


namespace TransitPulse.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services)
    {
        services.AddSingleton<IRouteEventRepository,
            InMemoryRouteEventRepository>();

        services.AddSingleton<IReliabilitySnapshotRepository,
            InMemoryReliabilitySnapshotRepository>();

        services.AddSingleton<IReliabilityCalculator,
            ReliabiltyCalculator>();

        return services;
    }
}