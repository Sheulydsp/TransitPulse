namespace TransitPulse.Domain.Entities;

public class ReliabilitySnapshot
{
    public Guid Id { get; private set; }
    public Guid RouteId { get; private set; }

    // Reliability score calculated from historical route events.
    public double Score { get; private set; }

    // Average delay observed during the calculation period.
    public double AverageDelay { get; private set; }

    // Percentage of cancelled trips during the calculation period.
    public double CancellationRate { get; private set; }

    // Indicates when the reliability metrics were calculated.
    public DateTime CalculatedAt { get; private set; }

    public ReliabilitySnapshot(
        Guid routeId,
        double score,
        double averageDelay,
        double cancellationRate,
        DateTime calculatedAt)
    {
        RouteId = routeId;
        Score = score;
        AverageDelay = averageDelay;
        CancellationRate = cancellationRate;
        CalculatedAt = calculatedAt;
    }
}