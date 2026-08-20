using FluentAssertions;

namespace TransitPulse.API.Tests.Infrastructure;

public class ApiFactoryTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public ApiFactoryTests(
        CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Factory_should_start_successfully()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(
            "/api/Reliability/health");

        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.IsSuccessStatusCode
            .Should()
            .BeTrue(
                $"status code was {response.StatusCode}, response body was: {content}");
    }
}