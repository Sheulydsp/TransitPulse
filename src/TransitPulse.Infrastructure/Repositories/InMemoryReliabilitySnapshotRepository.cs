using TransitPulse.Application.Interfaces;
using TransitPulse.Domain.Entities;

namespace TransitPulse.Infrastructure.Repositories;

public class InMemoryReliabilitySnapshotRepository : IReliabilitySnapshotRepository
{
    private readonly List<ReliabilitySnapshot> _snapshots = [];

    public Task AddAsync(
        ReliabilitySnapshot snapshot,
        CancellationToken cancellationToken)
    {
        _snapshots.Add(snapshot);

        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<ReliabilitySnapshot>> GetByRouteAsync(Guid routeId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}