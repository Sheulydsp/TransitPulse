using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TransitPulse.Domain.Entities;
using TransitPulse.Infrastructure.Identity;

namespace TransitPulse.Infrastructure.Persistence;

public class TransitPulseDbContext : IdentityDbContext<ApplicationUser>
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
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(TransitPulseDbContext).Assembly);
    }


}