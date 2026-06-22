using TransitPulse.Application.Exceptions;
using TransitPulse.Application.Interfaces;

namespace TransitPulse.Application.Features.Reliability.GetReliabilitySnapshots
{
    public class GetReliabilitySnapshotsHandler
    {
        private readonly IReliabilitySnapshotRepository
            _snapshotRepository;

        public GetReliabilitySnapshotsHandler(
            IReliabilitySnapshotRepository snapshotRepository)
        {
            _snapshotRepository = snapshotRepository;
        }

        public async Task<IReadOnlyList<GetReliabilitySnapshotsResult>> HandleAsync(Guid routeId, CancellationToken cancellationToken)
        {
            var snapshots = await _snapshotRepository.GetByRouteAsync(routeId, cancellationToken);

            if (!snapshots.Any())
            {
                throw new NotFoundException(
                    $"No snapshots found for route {routeId}");
            }
            return snapshots.Select(
                snapshot => new GetReliabilitySnapshotsResult(
                    snapshot.Id,
                    snapshot.Score,
                    snapshot.AverageDelay,
                    snapshot.CancellationRate,
                    snapshot.OnTimePercentage,
                    snapshot.CalculatedAt)).ToList();

        }
    }
}