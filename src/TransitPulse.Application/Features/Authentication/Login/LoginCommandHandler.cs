using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using TransitPulse.Application.Features.Authentication.Common;
using TransitPulse.Application.Interfaces;

namespace TransitPulse.Application.Features.Authentication.Login;

public class LoginCommandHandler
    : IRequestHandler<LoginCommand, LoginResult>
{
    private readonly IIdentityService _identityService;
    private readonly IValidator<LoginCommand> _validator;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(
        IIdentityService identityService,
        IValidator<LoginCommand> validator,
        ILogger<LoginCommandHandler> logger)
    {
        _identityService = identityService;
        _validator = validator;
        _logger = logger;
    }

    public async Task<LoginResult> Handle(
        LoginCommand command,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Processing login request for {Email}.",
            command.Email);

        await _validator.ValidateAndThrowAsync(
            command,
            cancellationToken);

        var request = new LoginUserRequest(
            command.Email,
            command.Password);

        var authenticationResult =
            await _identityService.LoginAsync(
                request,
                cancellationToken);

        return new LoginResult(
            authenticationResult.UserId,
            authenticationResult.FullName,
            authenticationResult.Email,
            authenticationResult.Token);
    }
}