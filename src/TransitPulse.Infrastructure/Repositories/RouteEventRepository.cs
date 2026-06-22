using Microsoft.EntityFrameworkCore;
using TransitPulse.Application.Interfaces;
using TransitPulse.Domain.Entities;
using TransitPulse.Infrastructure.Persistence;

public class RouteEventRepository : IRouteEventRepository
{
    private readonly TransitPulseDbContext _context;


    public RouteEventRepository(TransitPulseDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<RouteEvent>>
        GetByRouteAndPeriodAsync(
            Guid routeId,
            DateTime periodStart,
            DateTime periodEnd,
            CancellationToken cancellationToken)
    {
        return await _context.RouteEvents
            .Where(routeEvent =>
                routeEvent.RouteId == routeId &&
                routeEvent.ScheduledTime >= periodStart &&
                routeEvent.ScheduledTime <= periodEnd)
            .ToListAsync(cancellationToken);
    }
}