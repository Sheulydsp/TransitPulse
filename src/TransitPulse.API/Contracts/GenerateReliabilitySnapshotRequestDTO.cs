namespace TransitPulse.API.Contracts;

public record GenerateReliabilitySnapshotRequestDTO(
    Guid RouteId,
    DateTime PeriodStart,
    DateTime PeriodEnd

);
