using FluentAssertions;
using TransitPulse.Application.Features.Authentication.Login;

namespace TransitPulse.Application.Tests.Authentication.Login;

public class LoginCommandValidatorTests
{
    private readonly LoginCommandValidator _validator;

    public LoginCommandValidatorTests()
    {
        _validator = new LoginCommandValidator();
    }

    [Fact]
    public void Should_be_valid_when_login_data_is_valid()
    {
        // Arrange
        var command = new LoginCommand(
            "test@transitpulse.com",
            "Password123!");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_be_invalid_when_email_is_empty()
    {
        // Arrange
        var command = new LoginCommand(
            string.Empty,
            "Password123!");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(LoginCommand.Email));
    }

    [Fact]
    public void Should_be_invalid_when_email_format_is_invalid()
    {
        // Arrange
        var command = new LoginCommand(
            "invalid-email",
            "Password123!");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(LoginCommand.Email));
    }

    [Fact]
    public void Should_be_invalid_when_password_is_empty()
    {
        // Arrange
        var command = new LoginCommand(
            "test@transitpulse.com",
            string.Empty);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error =>
            error.PropertyName == nameof(LoginCommand.Password));
    }
}