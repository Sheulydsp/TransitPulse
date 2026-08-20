using MediatR;

namespace TransitPulse.Application.Features.Authentication.Register;

public record RegisterCommand(
    string FullName,
    string Email,
    string Password)
    : IRequest<RegisterResult>;