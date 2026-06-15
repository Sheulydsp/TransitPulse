namespace TransitPulse.Domain.Entities;

/// <summary>
/// Represents reliability analytics for a route
/// during a specific time period.
///
/// Historical snapshots support trend analysis,
/// route rankings, prediction models,
/// and future AI capabilities.
/// </summary>
public class ReliabilitySnapshot
{
    public Guid Id { get; private set; }

    public Guid RouteId { get; private set; }

    // Overall reliability score (0-100).
    public double Score { get; private set; }

    // Average delay in minutes.
    public double AverageDelay { get; private set; }

    // Percentage of cancelled trips.
    public double CancellationRate { get; private set; }

    // Percentage of trips arriving on time.
    public double OnTimePercentage { get; private set; }

    // Period used for the calculation.
    public DateTime PeriodStart { get; private set; }

    public DateTime PeriodEnd { get; private set; }

    // Timestamp when the snapshot was generated.
    public DateTime CalculatedAt { get; private set; }

    private ReliabilitySnapshot()
    {

        // Used by EF Core when materializing entities
        // from the database.
    }

    public ReliabilitySnapshot(
        Guid routeId,
        double score,
        double averageDelay,
        double cancellationRate,
        double onTimePercentage,
        DateTime periodStart,
        DateTime periodEnd,
        DateTime calculatedAt)
    {

        if (score < 0 || score > 100)
            throw new ArgumentException(
                "Score must be between 0 and 100.",
                nameof(score));

        if (averageDelay < 0)
            throw new ArgumentException(
                "Average delay cannot be negative.",
                nameof(averageDelay));

        if (cancellationRate < 0 || cancellationRate > 100)
            throw new ArgumentException(
                "Cancellation rate must be between 0 and 100.",
                nameof(cancellationRate));

        if (onTimePercentage < 0 || onTimePercentage > 100)
            throw new ArgumentException(
                "On-time percentage must be between 0 and 100.",
                nameof(onTimePercentage));

        if (periodEnd < periodStart)
            throw new ArgumentException(
                "Period end must be after period start.");

        Id = Guid.NewGuid();
        RouteId = routeId;
        Score = score;
        AverageDelay = averageDelay;
        CancellationRate = cancellationRate;
        OnTimePercentage = onTimePercentage;
        PeriodStart = periodStart;
        PeriodEnd = periodEnd;
        CalculatedAt = calculatedAt;
    }
}