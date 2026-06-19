using Microsoft.EntityFrameworkCore;
using TransitPulse.Domain.Entities;

namespace TransitPulse.Infrastructure.Persistence;

public class TransitPulseDbContext : DbContext
{
    public TransitPulseDbContext(
        DbContextOptions<TransitPulseDbContext> options) : base(options)
    {
    }

    public DbSet<ReliabilitySnapshot>
        ReliabilitySnapshots => Set<ReliabilitySnapshot>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(TransitPulseDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }


}