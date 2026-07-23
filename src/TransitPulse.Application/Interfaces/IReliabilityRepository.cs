using TransitPulse.Application.Features.Reliability.GetReliability;

namespace TransitPulse.Application.Interfaces;

public interface IReliabilityRepository
{
    Task<ReliabilityDto?> GetByRouteIdAsync(
    Guid routeId,
    CancellationToken cancellationToken);


}
