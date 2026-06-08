namespace TransitPulse.Application.Features.Reliability.GenerateReliabilitySnapshot;

public record GenerateReliabilitySnapshotCommand(
    Guid RouteId,
    DateTime PeriodStart,
    DateTime PeriodEnd);
