namespace TransitPulse.API.Contracts.Responses;

public record GetReliabilitySnapshotResponse(
   Guid SnapshotId,
   double Score,
   double AverageDelay,
   double CancellationRate,
   double OnTimePercentage,
   DateTime CalculatedAt
);
