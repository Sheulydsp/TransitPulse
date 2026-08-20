using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using TransitPulse.Application.Features.Authentication.Common;
using TransitPulse.Application.Interfaces;

namespace TransitPulse.Application.Features.Authentication.Register;

public class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, RegisterResult>
{
    private readonly IIdentityService _identityService;
    private readonly IValidator<RegisterCommand> _validator;
    private readonly ILogger<RegisterCommandHandler> _logger;

    public RegisterCommandHandler(
        IIdentityService identityService,
        IValidator<RegisterCommand> validator,
        ILogger<RegisterCommandHandler> logger)
    {
        _identityService = identityService;
        _validator = validator;
        _logger = logger;
    }

    public async Task<RegisterResult> Handle(
        RegisterCommand command,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Registering user with email {Email}.",
            command.Email);

        await _validator.ValidateAndThrowAsync(
            command,
            cancellationToken);

        var request = new RegisterUserRequest(
            command.FullName,
            command.Email,
            command.Password);

        var authenticationResult =
            await _identityService.RegisterAsync(
                request,
                cancellationToken);

        return new RegisterResult(
            authenticationResult.UserId,
            authenticationResult.FullName,
            authenticationResult.Email,
            authenticationResult.Token);
    }
}