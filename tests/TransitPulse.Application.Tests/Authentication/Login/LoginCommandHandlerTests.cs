using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using TransitPulse.Application.Features.Authentication.Common;
using TransitPulse.Application.Features.Authentication.Login;
using TransitPulse.Application.Interfaces;

namespace TransitPulse.Application.Tests.Authentication.Login;

public class LoginCommandHandlerTests
{
    private readonly Mock<IIdentityService> _identityServiceMock;
    private readonly Mock<ILogger<LoginCommandHandler>> _loggerMock;
    private readonly LoginCommandValidator _validator;
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        _identityServiceMock = new Mock<IIdentityService>();
        _loggerMock = new Mock<ILogger<LoginCommandHandler>>();
        _validator = new LoginCommandValidator();

        _handler = new LoginCommandHandler(
            _identityServiceMock.Object,
            _validator,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Should_login_user_when_command_is_valid()
    {
        // Arrange
        var command = new LoginCommand(
            "test@transitpulse.com",
            "Password123!");

        var authenticationResult = new AuthenticationResult(
            "user-123",
            "TransitPulse Test User",
            "test@transitpulse.com",
            "jwt-token");

        _identityServiceMock
            .Setup(service => service.LoginAsync(
                It.IsAny<LoginUserRequest>(),
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
            service => service.LoginAsync(
                It.Is<LoginUserRequest>(request =>
                    request.Email == command.Email &&
                    request.Password == command.Password),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Should_not_call_identity_service_when_command_is_invalid()
    {
        // Arrange
        var command = new LoginCommand(
            string.Empty,
            string.Empty);

        // Act
        var act = async () =>
            await _handler.Handle(
                command,
                CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<ValidationException>();

        _identityServiceMock.Verify(
            service => service.LoginAsync(
                It.IsAny<LoginUserRequest>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}