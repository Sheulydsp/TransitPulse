namespace TransitPulse.Application.Features.Reliability.GetReliabilitySnapshot;

public record GetReliabilitySnapshotResult(
    Guid SnapshotId,
    double Score,
    double AverageDelay,
    double CancellationRate,
    double OnTimePercentage,
    DateTime CalculatedAt);