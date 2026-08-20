using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TransitPulse.Application.Features.Authentication.Login;
using TransitPulse.Application.Features.Authentication.Register;
using TransitPulse.Domain.Entities;
using TransitPulse.Infrastructure.Persistence;
using TransitPulse.API.Tests.Infrastructure;
using TransitPulse.API.Contracts.Responses;

namespace TransitPulse.API.Tests.Reliability;

public class ReliabilityControllerTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public ReliabilityControllerTests(
        CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Authenticated_user_should_get_snapshots_for_route()
    {
        // Arrange

        var email =
            $"reliability-{Guid.NewGuid()}@transitpulse.com";

        var password = "Password123!";

        await _client.PostAsJsonAsync(
            "/api/Auth/register",
            new RegisterCommand(
                "Reliability Test User",
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

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                loginResult!.Token);

        // Create test data directly in PostgreSQL

        using var scope =
            _factory.Services.CreateScope();

        var dbContext =
            scope.ServiceProvider
                .GetRequiredService<TransitPulseDbContext>();

        var route =
            new TransitPulse.Domain.Entities.Route(
                "TEST-GET",
                "Reliability Test Route",
                TransitPulse.Domain.Enums.TransportType.Bus);

        dbContext.Routes.Add(route);

        var snapshot =
            new ReliabilitySnapshot(
                route.Id,
                92,
                2.5,
                5,
                92.5,
                DateTime.UtcNow.AddDays(-7),
                DateTime.UtcNow,
                DateTime.UtcNow);

        dbContext.ReliabilitySnapshots.Add(snapshot);

        await dbContext.SaveChangesAsync();

        // Act

        var response =
            await _client.GetAsync(
                $"/api/Reliability/routes/{route.Id}/snapshots");

        // Assert

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        var result =
            await response.Content
                .ReadFromJsonAsync<
                    List<GetReliabilitySnapshotResponse>>();

        result.Should().NotBeNull();

        result!
            .Should()
            .ContainSingle();

        result[0].SnapshotId
            .Should()
            .Be(snapshot.Id);

        result[0].Score
            .Should()
            .Be(92);

        result[0].AverageDelay
            .Should()
            .Be(2.5);

        result[0].CancellationRate
            .Should()
            .Be(5);

        result[0].OnTimePercentage
            .Should()
            .Be(92.5);
    }

    [Fact]
    public async Task Authenticated_user_should_get_snapshot_by_id()
    {
        // Arrange

        var email =
            $"single-snapshot-{Guid.NewGuid()}@transitpulse.com";

        var password = "Password123!";

        await _client.PostAsJsonAsync(
            "/api/Auth/register",
            new RegisterCommand(
                "Snapshot Test User",
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

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                loginResult!.Token);

        using var scope =
            _factory.Services.CreateScope();

        var dbContext =
            scope.ServiceProvider
                .GetRequiredService<TransitPulseDbContext>();

        var route =
            new TransitPulse.Domain.Entities.Route(
                "TEST-SINGLE",
                "Single Snapshot Test Route",
                TransitPulse.Domain.Enums.TransportType.Bus);

        dbContext.Routes.Add(route);

        var snapshot =
            new ReliabilitySnapshot(
                route.Id,
                88,
                3.5,
                10,
                90,
                DateTime.UtcNow.AddDays(-7),
                DateTime.UtcNow,
                DateTime.UtcNow);

        dbContext.ReliabilitySnapshots.Add(snapshot);

        await dbContext.SaveChangesAsync();

        // Act

        var response =
            await _client.GetAsync(
                $"/api/Reliability/snapshots/{snapshot.Id}");

        // Assert

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        var result =
            await response.Content
                .ReadFromJsonAsync<GetReliabilitySnapshotResponse>();

        result.Should().NotBeNull();

        result!.SnapshotId
            .Should()
            .Be(snapshot.Id);

        result.Score
            .Should()
            .Be(88);

        result.AverageDelay
            .Should()
            .Be(3.5);

        result.CancellationRate
            .Should()
            .Be(10);

        result.OnTimePercentage
            .Should()
            .Be(90);
    }

    [Fact]
    public async Task Getting_nonexistent_snapshot_should_return_not_found()
    {
        // Arrange

        var email =
            $"missing-snapshot-{Guid.NewGuid()}@transitpulse.com";

        var password = "Password123!";

        await _client.PostAsJsonAsync(
            "/api/Auth/register",
            new RegisterCommand(
                "Missing Snapshot User",
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

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                loginResult!.Token);

        var snapshotId = Guid.NewGuid();

        // Act

        var response =
            await _client.GetAsync(
                $"/api/Reliability/snapshots/{snapshotId}");

        // Assert

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }
}