using TransitPulse.Domain.Entities;

namespace TransitPulse.Application.Interfaces;

public interface IRouteEventRepository
{
    Task<IReadOnlyList<RouteEvent>> GetByRouteAndPeriodAsync(
    Guid routeId,
    DateTime periodStart,
    DateTime periodEnd,
    CancellationToken cancellationToken);
}
