using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using TransitPulse.Application.Exceptions;
using TransitPulse.Application.Features.Authentication.Common;
using TransitPulse.Application.Features.Authentication.Register;
using TransitPulse.Application.Interfaces;

namespace TransitPulse.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ILogger<IdentityService> _logger;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        IJwtTokenGenerator jwtTokenGenerator,
        ILogger<IdentityService> logger)
    {
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
        _logger = logger;
    }

    public async Task<AuthenticationResult> RegisterAsync(
        RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Attempting to register user with email {Email}.",
            request.Email);

        var existingUser =
            await _userManager.FindByEmailAsync(request.Email);

        if (existingUser is not null)
        {
            _logger.LogWarning(
                "Registration failed. Email {Email} already exists.",
                request.Email);

            throw new ConflictException(
                $"A user with email '{request.Email}' already exists.");
        }

        var user = new ApplicationUser
        {
            FullName = request.FullName,
            UserName = request.Email,
            Email = request.Email
        };

        var result = await _userManager.CreateAsync(
            user,
            request.Password);

        if (!result.Succeeded)
        {
            throw new BadRequestException(
                result.Errors.Select(
                    error => error.Description));
        }

        var roleResult = await _userManager.AddToRoleAsync(
            user,
            "User");

        if (!roleResult.Succeeded)
        {
            throw new BadRequestException(
                roleResult.Errors.Select(
                    error => error.Description));
        }

        var roles = await _userManager.GetRolesAsync(user);

        var token = _jwtTokenGenerator.GenerateToken(
            user.Id,
            user.Email!,
            roles);

        _logger.LogInformation(
            "User {Email} registered successfully.",
            request.Email);

        return new AuthenticationResult(
            user.Id,
            user.FullName,
            user.Email!,
            token);
    }

    public async Task<AuthenticationResult> LoginAsync(
        LoginUserRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Attempting login for user {Email}.",
            request.Email);

        var user = await _userManager.FindByEmailAsync(
            request.Email);

        if (user is null)
        {
            _logger.LogWarning(
                "Login failed for user {Email}.",
                request.Email);

            throw new UnauthorizedException(
                "Invalid email or password.");
        }

        var passwordValid =
            await _userManager.CheckPasswordAsync(
                user,
                request.Password);

        if (!passwordValid)
        {
            _logger.LogWarning(
                "Login failed for user {Email}.",
                request.Email);

            throw new UnauthorizedException(
                "Invalid email or password.");
        }

        var roles = await _userManager.GetRolesAsync(user);

        var token = _jwtTokenGenerator.GenerateToken(
            user.Id,
            user.Email!,
            roles);

        _logger.LogInformation(
            "User {Email} logged in successfully.",
            request.Email);

        return new AuthenticationResult(
            user.Id,
            user.FullName,
            user.Email!,
            token);
    }
}