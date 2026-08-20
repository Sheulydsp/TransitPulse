using FluentAssertions;
using TransitPulse.Application.Features.Authentication.Register;

namespace TransitPulse.Application.Tests.Authentication.Register;

public class RegisterCommandValidatorTests
{
    private readonly RegisterCommandValidator _validator;

    public RegisterCommandValidatorTests()
    {
        _validator = new RegisterCommandValidator();
    }

    [Fact]
    public void Should_be_valid_when_registration_data_is_valid()
    {
        // Arrange
        var command = new RegisterCommand(
            "TransitPulse Test User",
            "test@transitpulse.com",
            "Password123!");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_be_invalid_when_full_name_is_empty()
    {
        // Arrange
        var command = new RegisterCommand(
            string.Empty,
            "test@transitpulse.com",
            "Password123!");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(error =>
            error.PropertyName == nameof(RegisterCommand.FullName));
    }

    [Fact]
    public void Should_be_invalid_when_email_is_empty()
    {
        // Arrange
        var command = new RegisterCommand(
            "TransitPulse Test User",
            string.Empty,
            "Password123!");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(RegisterCommand.Email));
    }

    [Fact]
    public void Should_be_invalid_when_email_format_is_invalid()
    {
        // Arrange
        var command = new RegisterCommand(
            "TransitPulse Test User",
            "invalid-email",
            "Password123!");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(RegisterCommand.Email));
    }

    [Fact]
    public void Should_be_invalid_when_password_is_too_short()
    {
        // Arrange
        var command = new RegisterCommand(
            "TransitPulse Test User",
            "test@transitpulse.com",
            "Pass1!");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(RegisterCommand.Password));
    }

    [Fact]
    public void Should_be_invalid_when_password_has_no_uppercase_letter()
    {
        // Arrange
        var command = new RegisterCommand(
            "TransitPulse Test User",
            "test@transitpulse.com",
            "password123!");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(RegisterCommand.Password)
            && error.ErrorMessage.Contains("uppercase"));
    }

    [Fact]
    public void Should_be_invalid_when_password_has_no_lowercase_letter()
    {
        // Arrange
        var command = new RegisterCommand(
            "TransitPulse Test User",
            "test@transitpulse.com",
            "PASSWORD123!");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(RegisterCommand.Password)
            && error.ErrorMessage.Contains("lowercase"));
    }

    [Fact]
    public void Should_be_invalid_when_password_has_no_number()
    {
        // Arrange
        var command = new RegisterCommand(
            "TransitPulse Test User",
            "test@transitpulse.com",
            "Password!");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(RegisterCommand.Password)
            && error.ErrorMessage.Contains("number"));
    }

    [Fact]
    public void Should_be_invalid_when_password_has_no_special_character()
    {
        // Arrange
        var command = new RegisterCommand(
            "TransitPulse Test User",
            "test@transitpulse.com",
            "Password123");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(RegisterCommand.Password)
            && error.ErrorMessage.Contains("special character"));
    }
}