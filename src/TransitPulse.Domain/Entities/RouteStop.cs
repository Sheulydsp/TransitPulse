namespace TransitPulse.Domain.Entities;

public class RouteStop
{
    public Guid RouteId { get; private set; }

    public Guid StopId { get; private set; }

    // Preserves the order of stops within a route.
    // Example:
    // Route 31
    // 1. Jernbanetorget
    // 2. Nationaltheatret
    // 3. Majorstuen
    public int SequenceNumber { get; private set; }

    public RouteStop(Guid routeId, Guid stopId, int sequenceNumber)
    {
        if (sequenceNumber < 1)
            throw new ArgumentException(
                "Sequence number must be greater than zero.",
                nameof(sequenceNumber));

        RouteId = routeId;
        StopId = stopId;
        SequenceNumber = sequenceNumber;
    }

}