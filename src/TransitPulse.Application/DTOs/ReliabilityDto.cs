namespace TransitPulse.Application.DTOs;

/// <summary>
/// Reliability information returned to the client.
/// </summary>
public record ReliabilityDto(
    Guid RouteId,
    string RouteName,

    double Score,

    double AverageDelay,
    double CancellationRate,
    double OnTimePercentage,

    DateTime PeriodStart,
    DateTime PeriodEnd,

    DateTime CalculatedAt);