using FluentAssertions;
using Moq;
using TransitPulse.Application.Common;
using TransitPulse.Application.Exceptions;
using TransitPulse.Application.Features.Reliability.GenerateReliabilitySnapshot;
using TransitPulse.Application.Interfaces;
using TransitPulse.Domain.Entities;
using Microsoft.Extensions.Logging;


public class GenerateReliabilitySnapshotHandlerTests
{
    [Fact]
    public async Task Handle_WithNoRouteEvents_ShouldThrowInvalidOperationException()
    {
        // Arrange

        var routeEventRepository =
            new Mock<IRouteEventRepository>();

        var reliabilityCalculator =
            new Mock<IReliabilityCalculator>();

        var snapshotRepository =
            new Mock<IReliabilitySnapshotRepository>();

        var validator =
            new GenerateReliabilitySnapshotValidator();

        var logger =
            new Mock<ILogger<GenerateReliabilitySnapshotHandler>>();

        routeEventRepository
            .Setup(repository =>
                repository.GetByRouteAndPeriodAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<RouteEvent>());

        var handler =
            new GenerateReliabilitySnapshotHandler(
                routeEventRepository.Object,
                reliabilityCalculator.Object,
                snapshotRepository.Object,
                validator,
                logger.Object);

        var command =
            new GenerateReliabilitySnapshotCommand(
                Guid.NewGuid(),
                new DateTime(2026, 1, 1),
                new DateTime(2026, 1, 2));

        // Act

        Func<Task> action = () => handler.Handle(command, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_WithValidRouteEvents_ShouldSaveSnapshotAndReturnResult()
    {
        // Arrange

        var routeEventRepository =
            new Mock<IRouteEventRepository>();

        var reliabilityCalculator =
            new Mock<IReliabilityCalculator>();

        var snapshotRepository =
            new Mock<IReliabilitySnapshotRepository>();

        var validator =
            new GenerateReliabilitySnapshotValidator();

        var logger =
            new Mock<ILogger<GenerateReliabilitySnapshotHandler>>();

        var routeEvents =
            new List<RouteEvent>
            {
            new(
                Guid.NewGuid(),
                Guid.NewGuid(),
                new DateTime(2026, 1, 1, 10, 0, 0),
                new DateTime(2026, 1, 1, 10, 2, 0),
                false),

            new(
                Guid.NewGuid(),
                Guid.NewGuid(),
                new DateTime(2026, 1, 1, 11, 0, 0),
                new DateTime(2026, 1, 1, 11, 3, 0),
                false)
            };

        var metrics =
            new ReliabilityMetrics(
                2.5,
                10,
                90,
                92);

        routeEventRepository
            .Setup(repository =>
                repository.GetByRouteAndPeriodAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(routeEvents);

        reliabilityCalculator
            .Setup(calculator =>
                calculator.Calculate(routeEvents))
            .Returns(metrics);

        var handler =
            new GenerateReliabilitySnapshotHandler(
                routeEventRepository.Object,
                reliabilityCalculator.Object,
                snapshotRepository.Object,
                validator,
                logger.Object);

        var command =
            new GenerateReliabilitySnapshotCommand(
                Guid.NewGuid(),
                new DateTime(2026, 1, 1),
                new DateTime(2026, 1, 2));

        // Act

        var result =
            await handler.Handle(
                command,
                CancellationToken.None);

        // Assert

        result.Score.Should().Be(92);

        result.AverageDelay.Should().Be(2.5);

        result.CancellationRate.Should().Be(10);

        snapshotRepository.Verify(
            repository => repository.AddAsync(
                It.IsAny<ReliabilitySnapshot>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}