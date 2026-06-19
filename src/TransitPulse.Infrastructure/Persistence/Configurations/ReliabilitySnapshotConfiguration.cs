using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransitPulse.Domain.Entities;


namespace TransitPulse.Infrastructure.Persistence.Configurations;

public class ReliabilitySnapshotConfiguration : IEntityTypeConfiguration<ReliabilitySnapshot>
{
    public void Configure(EntityTypeBuilder<ReliabilitySnapshot> builder)
    {
        builder.ToTable(
            "reliability_snapshots");

        builder.HasKey(
            snapshot => snapshot.Id);

        builder.Property(
            snapshot => snapshot.Score)
            .IsRequired();

        builder.Property(
            snapshot => snapshot.AverageDelay)
            .IsRequired();

        builder.Property(
            snapshot => snapshot.CancellationRate)
            .IsRequired();

        builder.Property(
            snapshot => snapshot.OnTimePercentage)
            .IsRequired();

    }

}
