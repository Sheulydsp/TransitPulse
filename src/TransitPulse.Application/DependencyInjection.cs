using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TransitPulse.Application.Features.Reliability.GenerateReliabilitySnapshot;

namespace TransitPulse.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(
                typeof(ApplicationAssemblyMarker).Assembly);
        });

        services.AddValidatorsFromAssembly(
            typeof(ApplicationAssemblyMarker).Assembly);

        return services;
    }
}