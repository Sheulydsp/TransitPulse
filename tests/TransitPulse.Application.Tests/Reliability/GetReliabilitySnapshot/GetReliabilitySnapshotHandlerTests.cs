using FluentAssertions;
using FluentValidation;
using Moq;
using TransitPulse.Application.Exceptions;
using TransitPulse.Application.Features.Reliability.GenerateReliabilitySnapshot;
using TransitPulse.Application.Features.Reliability.GetReliabilitySnapshot;
using TransitPulse.Application.Features.Reliability.GetReliabilitySnapshots;
using TransitPulse.Application.Interfaces;
using TransitPulse.Domain.Entities;

namespace TransitPulse.Application.Tests.Reliability;

public class GetReliabilitySnapshotHandlerTests
{

    [Fact]
    public async Task HandleAsync_WithSnapshotNotFound_ShouldThrowNotFoundException()
    {
        // Arrange

        var snapshotRepository = new Mock<IReliabilitySnapshotRepository>();

        snapshotRepository.Setup(repository =>
                repository.GetByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync((ReliabilitySnapshot?)null);

        var handler = new GetReliabilitySnapshotHandler(snapshotRepository.Object);

        // Act

        Func<Task> action = () => handler.HandleAsync(
                                    Guid.NewGuid(),
                                    CancellationToken.None);

        // Assert

        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task HandleAsync_WithSnapshot_ShouldReturnMappedResult()
    {
        // Arrange

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
                new DateTime(2026, 1, 3));

        var snapshotRepository =
            new Mock<IReliabilitySnapshotRepository>();

        snapshotRepository.Setup(repository =>
                    repository.GetByIdAsync(
                    snapshot.Id,
                    It.IsAny<CancellationToken>())).ReturnsAsync(snapshot);

        var handler = new GetReliabilitySnapshotHandler(snapshotRepository.Object);

        // Act

        var result = await handler.HandleAsync(snapshot.Id, CancellationToken.None);

        // Assert

        result.Score.Should().Be(92);

        result.AverageDelay.Should().Be(2.5);

        result.CancellationRate.Should().Be(10);

        result.OnTimePercentage.Should().Be(90);

        result.CalculatedAt.Should().Be(snapshot.CalculatedAt);

        result.SnapshotId.Should().Be(snapshot.Id);
    }
}