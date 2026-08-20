using TransitPulse.Domain.Enums;

namespace TransitPulse.Domain.Entities;

// A route represents a transport line such as
// Bus 31, Metro 5, or Train L1.
public class Route
{
    public Guid Id { get; private set; }

    public string RouteCode { get; private set; }

    public string Name { get; private set; }

    public TransportType TransportType { get; private set; }
    public bool IsActive { get; private set; }

    public ICollection<ReliabilitySnapshot> ReliabilitySnapshots
    { get; private set; }
        = new List<ReliabilitySnapshot>();

    private Route()
    {
        // Required by EF Core
    }

    public Route(
        string routeCode,
        string name,
        TransportType transportType,
        bool isActive = true)
    {
        if (string.IsNullOrWhiteSpace(routeCode))
            throw new ArgumentException(
                "Route code is required.",
                nameof(routeCode));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Route name is required.",
                nameof(name));

        Id = Guid.NewGuid();
        RouteCode = routeCode;
        Name = name;
        TransportType = transportType;
        IsActive = isActive;
    }
}