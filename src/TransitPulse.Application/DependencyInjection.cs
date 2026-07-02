using Microsoft.Extensions.DependencyInjection;
using TransitPulse.Application.Features.Reliability.GenerateReliabilitySnapshot;
using TransitPulse.Application.Features.Reliability.GetReliabilitySnapshot;
using TransitPulse.Application.Features.Reliability.GetReliabilitySnapshots;

namespace TransitPulse.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        services.AddScoped<GetReliabilitySnapshotsHandler>();

        services.AddScoped<GetReliabilitySnapshotHandler>();

        services.AddScoped<GenerateReliabilitySnapshotHandler>();

        services.AddScoped<GenerateReliabilitySnapshotValidator>();

        return services;
    }
}