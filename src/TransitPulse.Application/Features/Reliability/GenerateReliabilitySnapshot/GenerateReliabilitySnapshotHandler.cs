using TransitPulse.Application.Interfaces;
using TransitPulse.Domain.Entities;

namespace TransitPulse.Application.Features.Reliability.GenerateReliabilitySnapshot;

public class GenerateReliabilitySnapshotHandler
{

    private readonly IRouteEventRepository _routeEventRepository;
    private readonly IReliabilityCalculator _reliabilityCalculator;
    private readonly IReliabilitySnapshotRepository _snapshotRepository;

    public GenerateReliabilitySnapshotHandler(
        IRouteEventRepository routeEventRepository,
        IReliabilityCalculator reliabilityCalculator,
        IReliabilitySnapshotRepository snapshotRepository)
    {
        _routeEventRepository = routeEventRepository;
        _reliabilityCalculator = reliabilityCalculator;
        _snapshotRepository = snapshotRepository;
    }

    public async Task<GenerateReliabilitySnapshotResult> HandleAsync(
    GenerateReliabilitySnapshotCommand command,
    CancellationToken cancellationToken)
    {
        var routeEvents =
            await _routeEventRepository
                .GetByRouteAndPeriodAsync(
                    command.RouteId,
                    command.PeriodStart,
                    command.PeriodEnd,
                    cancellationToken);

        if (!routeEvents.Any())
        {
            throw new InvalidOperationException(
                "No route events found for the specified period.");
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

        return new GenerateReliabilitySnapshotResult(
            snapshot.Id,
            metrics.Score,
            metrics.AverageDelay,
            metrics.CancellationRate);
    }
}
