using TransitPulse.Application.Exceptions;
using TransitPulse.Application.Interfaces;

namespace TransitPulse.Application.Features.Reliability.GetReliabilitySnapshot;

public class GetReliabilitySnapshotHandler
{
    private readonly IReliabilitySnapshotRepository _snapshotRepository;

    public GetReliabilitySnapshotHandler(
        IReliabilitySnapshotRepository snapshotRepository)
    {
        _snapshotRepository = snapshotRepository;
    }

    public async Task<GetReliabilitySnapshotResult> HandleAsync(
        Guid snapshotId,
        CancellationToken cancellationToken)
    {
        var snapshot = await _snapshotRepository.GetByIdAsync(
            snapshotId,
            cancellationToken);

        if (snapshot is null)
        {
            throw new NotFoundException(
                $"Snapshot {snapshotId} was not found.");
        }

        return new GetReliabilitySnapshotResult(
            snapshot.Id,
            snapshot.Score,
            snapshot.AverageDelay,
            snapshot.CancellationRate,
            snapshot.OnTimePercentage,
            snapshot.CalculatedAt);
    }
}