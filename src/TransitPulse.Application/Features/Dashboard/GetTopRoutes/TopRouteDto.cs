namespace TransitPulse.Application.Features.Dashboard.GetTopRoutes;

public record TopRouteDto(
    Guid RouteId,
    string RouteCode,
    string RouteName,
    string TransportType,
    double AverageScore
);