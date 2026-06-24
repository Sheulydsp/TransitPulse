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
}