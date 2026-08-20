using TransitPulse.Application.Features.Authentication.Common;

namespace TransitPulse.Application.Interfaces;

public interface IIdentityService
{
    Task<AuthenticationResult> RegisterAsync(
        RegisterUserRequest request,
        CancellationToken cancellationToken);

    Task<AuthenticationResult> LoginAsync(
        LoginUserRequest request,
        CancellationToken cancellationToken);
}