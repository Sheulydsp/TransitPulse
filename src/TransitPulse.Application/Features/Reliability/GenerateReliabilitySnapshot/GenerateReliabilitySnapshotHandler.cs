using TransitPulse.Application.Interfaces;

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

    public Task<GenerateReliabilitySnapshotResult> HandleAsync(
        GenerateReliabilitySnapshotCommand command,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
