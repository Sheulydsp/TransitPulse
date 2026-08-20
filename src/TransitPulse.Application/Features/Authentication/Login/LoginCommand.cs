using MediatR;

namespace TransitPulse.Application.Features.Authentication.Login;

public record LoginCommand(
    string Email,
    string Password)
    : IRequest<LoginResult>;