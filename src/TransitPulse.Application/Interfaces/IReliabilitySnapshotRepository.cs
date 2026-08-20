using TransitPulse.Application.Features.Dashboard.GetTopRoutes;
using TransitPulse.Domain.Entities;

namespace TransitPulse.Application.Interfaces;

public interface IReliabilitySnapshotRepository
{
    Task AddAsync(ReliabilitySnapshot snapshot, CancellationToken cancellationToken);
    Task<IReadOnlyList<ReliabilitySnapshot>> GetByRouteAsync(Guid routeId, CancellationToken cancellationToken);

    Task<ReliabilitySnapshot?> GetByIdAsync(Guid snapshotId, CancellationToken cancellationToken);

    Task<List<TopRouteDto>> GetTopRoutesAsync(CancellationToken cancellationToken);

}
