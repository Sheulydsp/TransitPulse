namespace TransitPulse.Application.Features.Reliability.GenerateReliabilitySnapshot;


/// <summary>
/// Result of reliability snapshot generation.
/// </summary>
public record GenerateReliabilitySnapshotResult(
    Guid SnapshotId,
    double Score,
    double AverageDelay,
    double CancellationRate);
