using TransitPulse.Application.Interfaces;
using TransitPulse.Domain.Entities;

namespace TransitPulse.Infrastructure.Repositories;

public class InMemoryRouteEventRepository : IRouteEventRepository
{
    private readonly List<RouteEvent> _routeEvents =
    [
        new RouteEvent(
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Guid.NewGuid(),
            DateTime.UtcNow.AddMinutes(-5),
            DateTime.UtcNow.AddMinutes(-3),
            false),

        new RouteEvent(
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Guid.NewGuid(),
            DateTime.UtcNow.AddMinutes(-10),
            DateTime.UtcNow.AddMinutes(-7),
            false),

        new RouteEvent(
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Guid.NewGuid(),
            DateTime.UtcNow.AddMinutes(-15),
            DateTime.UtcNow.AddMinutes(-15),
            true)
    ];

    public Task<IReadOnlyList<RouteEvent>> GetByRouteAndPeriodAsync(
        Guid routeId,
        DateTime periodStart,
        DateTime periodEnd,
        CancellationToken cancellationToken)
    {
        var events =
            _routeEvents
                .Where(e =>
                    e.RouteId == routeId &&
                    e.ScheduledTime >= periodStart &&
                    e.ScheduledTime <= periodEnd)
                .ToList();

        return Task.FromResult<IReadOnlyList<RouteEvent>>(events);
    }
}