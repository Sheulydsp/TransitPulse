using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TransitPulse.Domain.Entities;
using TransitPulse.Infrastructure.Persistence;

namespace TransitPulse.Infrastructure.Tests.Repositories;

public class ReliabilitySnapshotRepositoryTests
{
    [Fact]
    public async Task AddAsync_ShouldPersistSnapshot()
    {
        // Arrange

        var options =
            new DbContextOptionsBuilder<TransitPulseDbContext>()
                .UseInMemoryDatabase(
                    Guid.NewGuid().ToString())
                .Options;

        await using var context =
            new TransitPulseDbContext(options);

        var repository =
            new ReliabilitySnapshotRepository(
                context);

        var routeId = Guid.NewGuid();

        var snapshot =
            new ReliabilitySnapshot(
                routeId,
                92,
                2.5,
                10,
                90,
                new DateTime(2026, 1, 1),
                new DateTime(2026, 1, 2),
                DateTime.UtcNow);

        // Act

        await repository.AddAsync(
            snapshot,
            CancellationToken.None);

        // Assert

        var savedSnapshot =
            await context.ReliabilitySnapshots
                .FirstOrDefaultAsync();

        savedSnapshot.Should().NotBeNull();

        savedSnapshot!.RouteId
            .Should()
            .Be(routeId);

        savedSnapshot.Score
            .Should()
            .Be(92);
    }

    [Fact]
    public async Task GetByRouteAsync_ShouldReturnSnapshotsForRouteOrderedByCalculatedAt()
    {
        // Arrange

        var options =
            new DbContextOptionsBuilder<TransitPulseDbContext>()
                .UseInMemoryDatabase(
                    Guid.NewGuid().ToString())
                .Options;

        await using var context =
            new TransitPulseDbContext(options);

        var repository =
            new ReliabilitySnapshotRepository(
                context);

        var routeId = Guid.NewGuid();
        var otherRouteId = Guid.NewGuid();

        var olderSnapshot =
            new ReliabilitySnapshot(
                routeId,
                80,
                5,
                15,
                75,
                new DateTime(2026, 1, 1),
                new DateTime(2026, 1, 2),
                new DateTime(2026, 1, 3));

        var newerSnapshot =
            new ReliabilitySnapshot(
                routeId,
                90,
                2,
                5,
                95,
                new DateTime(2026, 1, 4),
                new DateTime(2026, 1, 5),
                new DateTime(2026, 1, 6));

        var differentRouteSnapshot =
            new ReliabilitySnapshot(
                otherRouteId,
                100,
                0,
                0,
                100,
                new DateTime(2026, 1, 7),
                new DateTime(2026, 1, 8),
                new DateTime(2026, 1, 9));

        await context.ReliabilitySnapshots.AddRangeAsync(
            olderSnapshot,
            newerSnapshot,
            differentRouteSnapshot);

        await context.SaveChangesAsync();

        // Act

        var result =
            await repository.GetByRouteAsync(
                routeId,
                CancellationToken.None);

        // Assert

        result.Should().HaveCount(2);

        result[0].Id.Should().Be(
            newerSnapshot.Id);

        result[1].Id.Should().Be(
            olderSnapshot.Id);

        result.Should()
            .OnlyContain(
                snapshot =>
                    snapshot.RouteId == routeId);
    }
}