using FluentAssertions;
using TransitPulse.Domain.Entities;
using TransitPulse.Infrastructure.Services;

namespace TransitPulse.Application.Tests.Reliability;

public class ReliabiltyCalculatorTests
{
    [Fact]
    public void Calculate_ShouldReturnExpectedMetrics()
    {
        // Arrange
        var calculator = new ReliabiltyCalculator();

        var routeEvents = new List<RouteEvent>
        {
            new RouteEvent(
                Guid.NewGuid(),
                Guid.NewGuid(),
                DateTime.UtcNow,
                DateTime.UtcNow,
                false),

            new RouteEvent(
                Guid.NewGuid(),
                Guid.NewGuid(),
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(5),
                false),

            new RouteEvent(
                Guid.NewGuid(),
                Guid.NewGuid(),
                DateTime.UtcNow,
                DateTime.UtcNow,
                true)
        };

        // Act
        var result = calculator.Calculate(routeEvents);

        // Assert
        result.AverageDelay.Should().Be(2.5);

        result.CancellationRate.Should()
            .BeApproximately(33.33333333333333, 0.001);

        result.OnTimePercentage.Should().Be(50);

        result.Score.Should()
            .BeApproximately(56.66666666666667, 0.001);
    }

    [Fact]
    public void Calculate_WithNoEvents_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var calculator = new ReliabiltyCalculator();

        // Act
        Action action =
            () => calculator.Calculate(
                Enumerable.Empty<RouteEvent>());

        // Assert
        action.Should()
            .Throw<InvalidOperationException>();
    }
}