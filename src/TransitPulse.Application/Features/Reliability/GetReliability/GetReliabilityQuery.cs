namespace TransitPulse.Application.Features.Reliability.GetReliability;

/// <summary>
/// Request to retrieve reliability metrics for a route.
/// </summary>
public record GetReliabilityQuery(
    Guid RouteId);