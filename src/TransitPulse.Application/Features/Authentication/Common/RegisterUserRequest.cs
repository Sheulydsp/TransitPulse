namespace TransitPulse.Application.Features.Authentication.Common;

public record RegisterUserRequest(
    string FullName,
    string Email,
    string Password);