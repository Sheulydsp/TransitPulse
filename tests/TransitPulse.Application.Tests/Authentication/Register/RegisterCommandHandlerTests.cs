using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using TransitPulse.Application.Features.Authentication.Common;
using TransitPulse.Application.Features.Authentication.Register;
using TransitPulse.Application.Interfaces;

namespace TransitPulse.Application.Tests.Authentication.Register;

public class RegisterCommandHandlerTests
{
    private readonly Mock<IIdentityService> _identityServiceMock;
    private readonly Mock<ILogger<RegisterCommandHandler>> _loggerMock;
    private readonly RegisterCommandValidator _validator;
    private readonly RegisterCommandHandler _handler;

    public RegisterCommandHandlerTests()
    {
        _identityServiceMock = new Mock<IIdentityService>();
        _loggerMock = new Mock<ILogger<RegisterCommandHandler>>();
        _validator = new RegisterCommandValidator();

        _handler = new RegisterCommandHandler(
            _identityServiceMock.Object,
            _validator,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Should_register_user_when_command_is_valid()
    {
        // Arrange
        var command = new RegisterCommand(
            "TransitPulse Test User",
            "test@transitpulse.com",
            "Password123!");

        var authenticationResult = new AuthenticationResult(
            "user-123",
            "TransitPulse Test User",
            "test@transitpulse.com",
            "jwt-token");

        _identityServiceMock
            .Setup(service => service.RegisterAsync(
                It.IsAny<RegisterUserRequest>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(authenticationResult);

        // Act
        var result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.UserId.Should().Be("user-123");
        result.FullName.Should().Be("TransitPulse Test User");
        result.Email.Should().Be("test@transitpulse.com");
        result.Token.Should().Be("jwt-token");

        _identityServiceMock.Verify(
            service => service.RegisterAsync(
                It.Is<RegisterUserRequest>(request =>
                    request.FullName == command.FullName &&
                    request.Email == command.Email &&
                    request.Password == command.Password),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Should_not_call_identity_service_when_command_is_invalid()
    {
        // Arrange
        var command = new RegisterCommand(
            string.Empty,
            "invalid-email",
            "weak");

        // Act
        var act = async () =>
            await _handler.Handle(
                command,
                CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<ValidationException>();

        _identityServiceMock.Verify(
            service => service.RegisterAsync(
                It.IsAny<RegisterUserRequest>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}