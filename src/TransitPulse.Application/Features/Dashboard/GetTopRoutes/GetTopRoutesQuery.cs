using MediatR;

namespace TransitPulse.Application.Features.Dashboard.GetTopRoutes;

public record GetTopRoutesQuery : IRequest<List<TopRouteDto>>;