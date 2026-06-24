using FluentAssertions;
using FluentValidation;
using TransitPulse.Application.Features.Reliability.GenerateReliabilitySnapshot;

namespace TransitPulse.Application.Tests.Reliability;

public class GenerateReliabilitySnapshotValidatorTests
{
    [Fact]
    public void Validate_WithValidCommand_ShouldNotThrow()
    {
        // Arrange
        var validator = new GenerateReliabilitySnapshotValidator();

        var command =
            new GenerateReliabilitySnapshotCommand(
                Guid.NewGuid(),
                new DateTime(2026, 1, 1),
                new DateTime(2026, 1, 2));

        // Act
        Action action =
            () => validator.ValidateAndThrow(command);

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithEmptyRouteId_ShouldThrowValidationException()
    {
        // Arrange
        var validator = new GenerateReliabilitySnapshotValidator();

        var command =
            new GenerateReliabilitySnapshotCommand(
                Guid.Empty,
                new DateTime(2026, 1, 1),
                new DateTime(2026, 1, 2));

        // Act
        Action action = () => validator.ValidateAndThrow(command);

        // Assert
        action.Should().Throw<ValidationException>();
    }
    [Fact]
    public void Validate_WithPeriodEndBeforePeriodStart_ShouldThrowValidationException()
    {
        // Arrange
        var validator = new GenerateReliabilitySnapshotValidator();

        var command =
            new GenerateReliabilitySnapshotCommand(
                Guid.NewGuid(),
                new DateTime(2026, 1, 2),
                new DateTime(2026, 1, 1));

        // Act
        Action action = () => validator.ValidateAndThrow(command);

        // Assert
        action.Should().Throw<ValidationException>();
    }
}