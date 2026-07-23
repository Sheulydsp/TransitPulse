namespace TransitPulse.Application.Features.Reliability.Common;

public record GetReliabilitySnapshotDto(
    Guid SnapshotId,
    double Score,
    double AverageDelay,
    double CancellationRate,
    double OnTimePercentage,
    DateTime CalculatedAt);