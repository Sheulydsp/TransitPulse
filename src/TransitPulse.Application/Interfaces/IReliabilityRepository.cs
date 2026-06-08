using TransitPulse.Application.DTOs;

namespace TransitPulse.Application.Interfaces;

    public interface IReliabilityRepository
    {
        Task<ReliabilityDto?> GetByRouteIdAsync(
        Guid routeId,
        CancellationToken cancellationToken);


    }
