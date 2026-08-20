namespace TransitPulse.Application.Features.Authentication.Common;

public record LoginUserRequest(
    string Email,
    string Password);