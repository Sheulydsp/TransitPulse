using Microsoft.EntityFrameworkCore;
using TransitPulse.Domain.Entities;
using TransitPulse.Domain.Enums;

namespace TransitPulse.Infrastructure.Persistence;

public class DbSeeder
{
    private readonly TransitPulseDbContext _context;

    public DbSeeder(TransitPulseDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        // Don't seed twice
        if (await _context.Routes.AnyAsync())
        {
            return;
        }

        // -------------------------
        // Seed Routes
        // -------------------------

        var routes = new List<Route>
        {
            new("B31", "Airport Express", TransportType.Bus),
            new("M1", "Metro Line 1", TransportType.Metro),
            new("T5", "Tram Line 5", TransportType.Tram),
            new("TR1", "Regional Train", TransportType.Train),
            new("F1", "Harbor Ferry", TransportType.Ferry)
        };

        _context.Routes.AddRange(routes);
        await _context.SaveChangesAsync();

        // -------------------------
        // Seed Route Events
        // -------------------------

        var random = new Random();
        var events = new List<RouteEvent>();

        foreach (var route in routes)
        {
            for (int i = 0; i < 20; i++)
            {
                var scheduled = DateTime.UtcNow
                    .AddDays(-random.Next(1, 30))
                    .AddHours(random.Next(0, 24))
                    .AddMinutes(random.Next(0, 60));

                bool cancelled = random.Next(100) < 5; // 5% cancellation

                DateTime actual = cancelled
                    ? scheduled
                    : scheduled.AddMinutes(random.Next(0, 15));

                events.Add(new RouteEvent(
                    route.Id,
                    Guid.NewGuid(), // Temporary StopId
                    scheduled,
                    actual,
                    cancelled));
            }
        }

        _context.RouteEvents.AddRange(events);
        await _context.SaveChangesAsync();
    }
}