using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using TransitPulse.Application.Exceptions;
using TransitPulse.Application.Interfaces;
using TransitPulse.Domain.Entities;

namespace TransitPulse.Application.Features.Reliability.GenerateReliabilitySnapshot;

public class GenerateReliabilitySnapshotHandler
    : IRequestHandler<
        GenerateReliabilitySnapshotCommand,
        GenerateReliabilitySnapshotResult>
{
    private readonly IRouteEventRepository _routeEventRepository;
    private readonly IReliabilityCalculator _reliabilityCalculator;
    private readonly IReliabilitySnapshotRepository _snapshotRepository;
    private readonly IValidator<GenerateReliabilitySnapshotCommand> _validator;
    private readonly ILogger<GenerateReliabilitySnapshotHandler> _logger;

    public GenerateReliabilitySnapshotHandler(
        IRouteEventRepository routeEventRepository,
        IReliabilityCalculator reliabilityCalculator,
        IReliabilitySnapshotRepository snapshotRepository,
        IValidator<GenerateReliabilitySnapshotCommand> validator,
        ILogger<GenerateReliabilitySnapshotHandler> logger)
    {
        _routeEventRepository = routeEventRepository;
        _reliabilityCalculator = reliabilityCalculator;
        _snapshotRepository = snapshotRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<GenerateReliabilitySnapshotResult> Handle(
        GenerateReliabilitySnapshotCommand command,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Starting reliability snapshot generation for route {RouteId}.",
            command.RouteId);



        _validator.ValidateAndThrow(command);

        // Normalize request timestamps to UTC.
        var periodStart = DateTime.SpecifyKind(
            command.PeriodStart,
            DateTimeKind.Utc);

        var periodEnd = DateTime.SpecifyKind(
            command.PeriodEnd,
            DateTimeKind.Utc);

        var routeEvents = await _routeEventRepository.GetByRouteAndPeriodAsync(
            command.RouteId,
            periodStart,
            periodEnd,
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

        var metrics = _reliabilityCalculator.Calculate(routeEvents);

        var snapshot = new ReliabilitySnapshot(
        command.RouteId,
        metrics.Score,
        metrics.AverageDelay,
        metrics.CancellationRate,
        metrics.OnTimePercentage,
        periodStart,
        periodEnd,
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