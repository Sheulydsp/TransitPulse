namespace TransitPulse.Application.Features.Authentication.Common;

public record AuthenticationResult(
    string UserId,
    string FullName,
    string Email,
    string Token);