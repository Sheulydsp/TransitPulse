namespace TransitPulse.Domain.Entities;

public class RouteEvent
{
    public Guid Id { get; private set; }

    public Guid RouteId { get; private set; }

    public Guid StopId { get; private set; }

    // Planned departure/arrival time from the transport schedule.
    public DateTime ScheduledTime { get; private set; }

    // Actual departure/arrival time reported by the transport operator.
    public DateTime ActualTime { get; private set; }

    // Indicates whether the trip was cancelled.
    public bool IsCancelled { get; private set; }

    public RouteEvent(
        Guid routeId,
        Guid stopId,
        DateTime scheduledTime,
        DateTime actualTime,
        bool isCancelled)
    {
        RouteId = routeId;
        StopId = stopId;
        ScheduledTime = scheduledTime;
        ActualTime = actualTime;
        IsCancelled = isCancelled;
    }

    /// Delay is calculated instead of stored to prevent
    // ScheduledTime, ActualTime, and DelayMinutes
    // from becoming inconsistent.
    public int DelayMinutes =>
        IsCancelled
            ? 0
            : (int)(ActualTime - ScheduledTime).TotalMinutes;

}
