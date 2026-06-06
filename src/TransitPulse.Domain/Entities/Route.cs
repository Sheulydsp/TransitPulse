using TransitPulse.Domain.Enums;

namespace TransitPulse.Domain.Entities
{
    public class Route
    {
        public Guid Id { get; set; }
        public string RouteCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public TransportType TransportType { get; set; }

    }
}