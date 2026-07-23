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

    /// <summary>
    /// Overall reliability score (0-100).
    /// </summary>
    public double Score { get; private set; }

    public Route Route { get; private set; } = null!;

    /// <summary>
    /// Average delay in minutes.
    /// </summary>
    public double AverageDelay { get; private set; }

    /// <summary>
    /// Percentage of cancelled trips.
    /// </summary>
    public double CancellationRate { get; private set; }

    /// <summary>
    /// Percentage of trips arriving on time.
    /// </summary>
    public double OnTimePercentage { get; private set; }

    /// <summary>
    /// Start of the reporting period (UTC).
    /// </summary>
    public DateTime PeriodStart { get; private set; }

    /// <summary>
    /// End of the reporting period (UTC).
    /// </summary>
    public DateTime PeriodEnd { get; private set; }

    /// <summary>
    /// Timestamp when the snapshot was generated (UTC).
    /// </summary>
    public DateTime CalculatedAt { get; private set; }

    private ReliabilitySnapshot()
    {
        // Required by EF Core.
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
            throw new ArgumentOutOfRangeException(
                nameof(score),
                "Score must be between 0 and 100.");

        if (averageDelay < 0)
            throw new ArgumentOutOfRangeException(
                nameof(averageDelay),
                "Average delay cannot be negative.");

        if (cancellationRate < 0 || cancellationRate > 100)
            throw new ArgumentOutOfRangeException(
                nameof(cancellationRate),
                "Cancellation rate must be between 0 and 100.");

        if (onTimePercentage < 0 || onTimePercentage > 100)
            throw new ArgumentOutOfRangeException(
                nameof(onTimePercentage),
                "On-time percentage must be between 0 and 100.");

        // Normalize all timestamps to UTC.
        periodStart = EnsureUtc(periodStart);
        periodEnd = EnsureUtc(periodEnd);
        calculatedAt = EnsureUtc(calculatedAt);

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

    private static DateTime EnsureUtc(DateTime value)
    {
        return value.Kind switch
        {
            DateTimeKind.Utc => value,
            DateTimeKind.Local => value.ToUniversalTime(),
            DateTimeKind.Unspecified => DateTime.SpecifyKind(
                value,
                DateTimeKind.Utc),
            _ => value
        };
    }
}