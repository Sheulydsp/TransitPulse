namespace TransitPulse.Application.Features.Authentication.Register;

/// <summary>
/// Result returned after a successful user registration.
/// </summary>
public record RegisterResult(
    string UserId,
    string FullName,
    string Email,
    string Token);