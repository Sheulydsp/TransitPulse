using TransitPulse.Domain.Entities;

namespace TransitPulse.Application.Interfaces;

public interface IReliabilitySnapshotRepository
{
    Task AddAsync(ReliabilitySnapshot snapshot, CancellationToken cancellationToken);
    Task<IReadOnlyList<ReliabilitySnapshot>> GetByRouteAsync(Guid routeId, CancellationToken cancellationToken);

}
