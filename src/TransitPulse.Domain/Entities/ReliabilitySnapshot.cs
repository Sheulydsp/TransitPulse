namespace TransitPulse.Domain.Entities;

/// <summary>
/// Represents reliability analytics for a route
/// during a specific period.
/// </summary>
public class ReliabilitySnapshot
{
    public Guid Id { get; private set; }

    public Guid RouteId { get; private set; }

    public double AverageDelay { get; private set; }

    public double CancellationRate { get; private set; }

    public double OnTimePercentage { get; private set; }

    public DateTime PeriodStart { get; private set; }

    public DateTime PeriodEnd { get; private set; }

    public DateTime CalculatedAt { get; private set; }

    public ReliabilitySnapshot(
        Guid routeId,
        double averageDelay,
        double cancellationRate,
        double onTimePercentage,
        DateTime periodStart,
        DateTime periodEnd,
        DateTime calculatedAt)
    {
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

        RouteId = routeId;
        AverageDelay = averageDelay;
        CancellationRate = cancellationRate;
        OnTimePercentage = onTimePercentage;
        PeriodStart = periodStart;
        PeriodEnd = periodEnd;
        CalculatedAt = calculatedAt;
    }
}