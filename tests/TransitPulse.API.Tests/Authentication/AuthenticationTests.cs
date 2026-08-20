using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using TransitPulse.API.Tests.Infrastructure;
using TransitPulse.Application.Features.Authentication.Common;
using TransitPulse.Application.Features.Authentication.Login;
using TransitPulse.Application.Features.Authentication.Register;
using TransitPulse.Domain.Entities;
using TransitPulse.Domain.Enums;
using TransitPulse.Infrastructure.Identity;
using TransitPulse.Infrastructure.Persistence;

namespace TransitPulse.API.Tests.Authentication;

public class AuthenticationTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public AuthenticationTests(
        CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_should_create_user_successfully()
    {
        // Arrange
        var request = new RegisterCommand(
            "Integration Test User",
            "integration-test@transitpulse.com",
            "Password123!");

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/Auth/register",
            request);

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        var result =
            await response.Content
                .ReadFromJsonAsync<RegisterResult>();

        result.Should().NotBeNull();

        result!.FullName
            .Should()
            .Be("Integration Test User");

        result.Email
            .Should()
            .Be("integration-test@transitpulse.com");

        result.UserId
            .Should()
            .NotBeNullOrWhiteSpace();

        result.Token
            .Should()
            .NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Login_should_return_jwt_for_valid_credentials()
    {
        // Arrange
        var email = $"login-{Guid.NewGuid()}@transitpulse.com";
        var password = "Password123!";

        var registerRequest = new RegisterCommand(
            "Login Integration User",
            email,
            password);

        var registerResponse = await _client.PostAsJsonAsync(
            "/api/Auth/register",
            registerRequest);

        registerResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        // Act
        var loginResponse = await _client.PostAsJsonAsync(
            "/api/Auth/login",
            new
            {
                email,
                password
            });

        // Assert
        loginResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        var result =
            await loginResponse.Content
                .ReadFromJsonAsync<LoginResult>();

        result.Should().NotBeNull();

        result!.UserId
            .Should()
            .NotBeNullOrWhiteSpace();

        result.Email
            .Should()
            .Be(email);

        result.FullName
            .Should()
            .Be("Login Integration User");

        result.Token
            .Should()
            .NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Login_should_return_unauthorized_for_invalid_password()
    {
        // Arrange
        var email = $"invalid-login-{Guid.NewGuid()}@transitpulse.com";

        var registerResponse = await _client.PostAsJsonAsync(
            "/api/Auth/register",
            new RegisterCommand(
                "Invalid Login User",
                email,
                "Password123!"));

        registerResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        // Act
        var loginResponse = await _client.PostAsJsonAsync(
            "/api/Auth/login",
            new
            {
                email,
                password = "WrongPassword123!"
            });

        // Assert
        loginResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Authenticated_user_should_access_protected_endpoint()
    {
        // Arrange
        var email =
            $"protected-{Guid.NewGuid()}@transitpulse.com";

        var password = "Password123!";

        await _client.PostAsJsonAsync(
            "/api/Auth/register",
            new RegisterCommand(
                "Protected Endpoint User",
                email,
                password));

        var loginResponse =
            await _client.PostAsJsonAsync(
                "/api/Auth/login",
                new
                {
                    email,
                    password
                });

        loginResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        var loginResult =
            await loginResponse.Content
                .ReadFromJsonAsync<LoginResult>();

        loginResult.Should().NotBeNull();

        loginResult!.Token
            .Should()
            .NotBeNullOrWhiteSpace();

        // Add JWT to Authorization header
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                loginResult.Token);

        var routeId = Guid.NewGuid();

        // Act
        var response =
            await _client.GetAsync(
                $"/api/Reliability/routes/{routeId}/snapshots");

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Regular_user_should_not_generate_reliability_snapshot()
    {
        // Arrange
        var email =
            $"user-admin-test-{Guid.NewGuid()}@transitpulse.com";

        var password = "Password123!";

        await _client.PostAsJsonAsync(
            "/api/Auth/register",
            new RegisterCommand(
                "Regular User",
                email,
                password));

        var loginResponse =
            await _client.PostAsJsonAsync(
                "/api/Auth/login",
                new
                {
                    email,
                    password
                });

        loginResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        var loginResult =
            await loginResponse.Content
                .ReadFromJsonAsync<LoginResult>();

        loginResult.Should().NotBeNull();

        loginResult!.Token
            .Should()
            .NotBeNullOrWhiteSpace();

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                loginResult.Token);

        var request = new
        {
            routeId = Guid.NewGuid(),
            periodStart = DateTime.UtcNow.AddDays(-7),
            periodEnd = DateTime.UtcNow
        };

        // Act
        var response =
            await _client.PostAsJsonAsync(
                "/api/Reliability/snapshots",
                request);

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Admin_user_should_generate_reliability_snapshot()
    {
        // Arrange
        var email =
            $"admin-test-{Guid.NewGuid()}@transitpulse.com";

        var password = "Password123!";

        // Register user through the real API
        var registerResponse =
            await _client.PostAsJsonAsync(
                "/api/Auth/register",
                new RegisterCommand(
                    "Integration Admin",
                    email,
                    password));

        registerResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        // Promote the user to Admin using ASP.NET Core Identity
        using var scope = _factory.Services.CreateScope();

        var userManager =
            scope.ServiceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();

        var user =
            await userManager.FindByEmailAsync(email);

        user.Should().NotBeNull();

        var roleResult =
            await userManager.AddToRoleAsync(
                user!,
                "Admin");

        roleResult.Succeeded
            .Should()
            .BeTrue();

        // Login again so the JWT contains the Admin role
        var loginResponse =
            await _client.PostAsJsonAsync(
                "/api/Auth/login",
                new
                {
                    email,
                    password
                });

        loginResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        var loginResult =
            await loginResponse.Content
                .ReadFromJsonAsync<LoginResult>();

        loginResult.Should().NotBeNull();

        loginResult!.Token
            .Should()
            .NotBeNullOrWhiteSpace();

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                loginResult.Token);

        using var dbScope = _factory.Services.CreateScope();

        var dbContext =
            dbScope.ServiceProvider
                .GetRequiredService<TransitPulseDbContext>();

        var route =
        new TransitPulse.Domain.Entities.Route(
        "TEST-ADMIN",
        "Admin Test Route",
        TransportType.Bus);

        dbContext.Routes.Add(route);

        await dbContext.SaveChangesAsync();

        var now = DateTime.UtcNow;

        var routeEvents = new[]
        {
        new RouteEvent(
            route.Id,
            Guid.NewGuid(),
            now.AddDays(-3),
            now.AddDays(-3).AddMinutes(2),
            false),

        new RouteEvent(
            route.Id,
            Guid.NewGuid(),
            now.AddDays(-2),
            now.AddDays(-2).AddMinutes(5),
            false),

        new RouteEvent(
            route.Id,
            Guid.NewGuid(),
            now.AddDays(-1),
            now.AddDays(-1),
            false)
        };

        dbContext.RouteEvents.AddRange(routeEvents);

        await dbContext.SaveChangesAsync();

        var request = new
        {
            routeId = route.Id,
            periodStart = DateTime.UtcNow.AddDays(-7),
            periodEnd = DateTime.UtcNow
        };

        // Act
        var response =
            await _client.PostAsJsonAsync(
                "/api/Reliability/snapshots",
                request);

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Created);
    }
}