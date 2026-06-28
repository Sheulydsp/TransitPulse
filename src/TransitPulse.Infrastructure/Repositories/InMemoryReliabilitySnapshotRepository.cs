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
    public Task<IReadOnlyList<ReliabilitySnapshot>> GetByRouteAsync(
        Guid routeId,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<ReliabilitySnapshot> snapshots = _snapshots
            .Where(snapshot => snapshot.RouteId == routeId)
            .OrderByDescending(snapshot => snapshot.CalculatedAt)
            .ToList();

        return Task.FromResult(snapshots);
    }

    public Task<ReliabilitySnapshot?> GetByIdAsync(
    Guid snapshotId,
    CancellationToken cancellationToken)
    {
        var snapshot = _snapshots.FirstOrDefault(
            snapshot => snapshot.Id == snapshotId);

        return Task.FromResult(snapshot);
    }
}