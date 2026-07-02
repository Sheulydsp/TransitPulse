using FluentValidation;
using TransitPulse.Application.Exceptions;
using TransitPulse.Application.Interfaces;
using TransitPulse.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace TransitPulse.Application.Features.Reliability.GenerateReliabilitySnapshot;

public class GenerateReliabilitySnapshotHandler
{

    private readonly IRouteEventRepository _routeEventRepository;
    private readonly IReliabilityCalculator _reliabilityCalculator;
    private readonly IReliabilitySnapshotRepository _snapshotRepository;
    private readonly GenerateReliabilitySnapshotValidator _validator;
    private readonly ILogger<GenerateReliabilitySnapshotHandler> _logger;

    public GenerateReliabilitySnapshotHandler(
        IRouteEventRepository routeEventRepository,
        IReliabilityCalculator reliabilityCalculator,
        IReliabilitySnapshotRepository snapshotRepository,
        GenerateReliabilitySnapshotValidator validator,
        ILogger<GenerateReliabilitySnapshotHandler> logger
        )
    {
        _routeEventRepository = routeEventRepository;
        _reliabilityCalculator = reliabilityCalculator;
        _snapshotRepository = snapshotRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<GenerateReliabilitySnapshotResult> HandleAsync(
    GenerateReliabilitySnapshotCommand command,
    CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Starting reliability snapshot generation for route {RouteId}.", command.RouteId);

        _validator.ValidateAndThrow(command);

        var routeEvents =
            await _routeEventRepository
                .GetByRouteAndPeriodAsync(
                    command.RouteId,
                    command.PeriodStart,
                    command.PeriodEnd,
                    cancellationToken);

        if (!routeEvents.Any())
        {
            _logger.LogWarning(
                "No route events found for route {RouteId} between {PeriodStart} and {PeriodEnd}.",
                command.RouteId,
                command.PeriodStart,
                command.PeriodEnd);

            throw new NotFoundException(
                $"No route events found for route {command.RouteId} between {command.PeriodStart:d} and {command.PeriodEnd:d}.");
        }
        //Single Responsibility Principle in IReliabilityCalculator calculate
        var metrics =
            _reliabilityCalculator.Calculate(
                routeEvents);

        var snapshot =
            new ReliabilitySnapshot(
                command.RouteId,
                metrics.Score,
                metrics.AverageDelay,
                metrics.CancellationRate,
                metrics.OnTimePercentage,
                command.PeriodStart,
                command.PeriodEnd,
                DateTime.UtcNow);

        await _snapshotRepository.AddAsync(
            snapshot,
            cancellationToken);

        _logger.LogInformation(
            "Reliability snapshot {SnapshotId} generated successfully for route {RouteId}.",
            snapshot.Id,
            command.RouteId);

        return new GenerateReliabilitySnapshotResult(
            snapshot.Id,
            metrics.Score,
            metrics.AverageDelay,
            metrics.CancellationRate);
    }
}
