namespace TransitPulse.Application.DTOs;

public record ReliabilityDto(
    double Score,
    double AverageDelay,
    double CancellationRate);