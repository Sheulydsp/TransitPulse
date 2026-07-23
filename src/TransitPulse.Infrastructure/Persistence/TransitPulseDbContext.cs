using Microsoft.EntityFrameworkCore;
using TransitPulse.Domain.Entities;

namespace TransitPulse.Infrastructure.Persistence;

public class TransitPulseDbContext : DbContext
{
    public TransitPulseDbContext(
        DbContextOptions<TransitPulseDbContext> options) : base(options)
    {
    }

    public DbSet<Route> Routes => Set<Route>();
    public DbSet<ReliabilitySnapshot>
        ReliabilitySnapshots => Set<ReliabilitySnapshot>();

    public DbSet<RouteEvent>
       RouteEvents => Set<RouteEvent>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(TransitPulseDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }


}