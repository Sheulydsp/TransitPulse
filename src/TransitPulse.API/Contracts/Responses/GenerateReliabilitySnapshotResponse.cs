namespace TransitPulse.API.Contracts.Responses;

public record GenerateReliabilitySnapshotResponse(
    Guid SnapshotId,
    double Score,
    double AverageDelay,
    double CancellationRate);