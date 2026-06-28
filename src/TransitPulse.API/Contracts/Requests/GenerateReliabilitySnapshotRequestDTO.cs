namespace TransitPulse.API.Contracts.Requests;

public record GenerateReliabilitySnapshotRequest(
    Guid RouteId,
    DateTime PeriodStart,
    DateTime PeriodEnd

);
