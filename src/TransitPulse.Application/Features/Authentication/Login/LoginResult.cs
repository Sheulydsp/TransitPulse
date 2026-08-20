namespace TransitPulse.Application.Features.Authentication.Login;

public record LoginResult(
    string UserId,
    string FullName,
    string Email,
    string Token);