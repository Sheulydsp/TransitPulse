namespace TransitPulse.Application.Features.Reliability.GetReliabilitySnapshots;

public record GetReliabilitySnapshotsResult(
    Guid SnapshotId,
    double Score,
    double AverageDelay,
    double CancellationRate,
    double OnTimePercentage,
    DateTime CalculatedAt);
