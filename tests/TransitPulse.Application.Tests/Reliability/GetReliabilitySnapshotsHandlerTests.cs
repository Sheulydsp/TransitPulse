using FluentAssertions;
using FluentValidation;
using Moq;
using TransitPulse.Application.Exceptions;
using TransitPulse.Application.Features.Reliability.GenerateReliabilitySnapshot;
using TransitPulse.Application.Features.Reliability.GetReliabilitySnapshots;
using TransitPulse.Application.Interfaces;
using TransitPulse.Domain.Entities;

namespace TransitPulse.Application.Tests.Reliability;

public class GetReliabilitySnapshotsHandlerTests
{

    [Fact]
    public async Task HandleAsync_WithNoSnapshots_ShouldThrowNotFoundException()
    {
        // Arrange

        var snapshotRepository =
            new Mock<IReliabilitySnapshotRepository>();

        snapshotRepository
            .Setup(repository =>
                repository.GetByRouteAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ReliabilitySnapshot>());

        var handler =
            new GetReliabilitySnapshotsHandler(
                snapshotRepository.Object);

        // Act

        Func<Task> action =
            () => handler.HandleAsync(
                Guid.NewGuid(),
                CancellationToken.None);

        // Assert

        await action.Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task HandleAsync_WithSnapshots_ShouldReturnMappedResults()
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

        snapshotRepository
            .Setup(repository =>
                repository.GetByRouteAsync(
                    routeId,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ReliabilitySnapshot>
            {
            snapshot
            });

        var handler =
            new GetReliabilitySnapshotsHandler(
                snapshotRepository.Object);

        // Act

        var result =
            await handler.HandleAsync(
                routeId,
                CancellationToken.None);

        // Assert

        result.Should().HaveCount(1);

        result[0].Score.Should().Be(92);

        result[0].AverageDelay.Should().Be(2.5);

        result[0].CancellationRate.Should().Be(10);

        result[0].OnTimePercentage.Should().Be(90);
    }
}