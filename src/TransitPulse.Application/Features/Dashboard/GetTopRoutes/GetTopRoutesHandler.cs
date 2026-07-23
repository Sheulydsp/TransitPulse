using MediatR;
using TransitPulse.Application.Interfaces;

namespace TransitPulse.Application.Features.Dashboard.GetTopRoutes;

public class GetTopRoutesHandler
    : IRequestHandler<GetTopRoutesQuery, List<TopRouteDto>>
{
    private readonly IReliabilitySnapshotRepository _repository;

    public GetTopRoutesHandler(IReliabilitySnapshotRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<TopRouteDto>> Handle(
        GetTopRoutesQuery request,
        CancellationToken cancellationToken)
    {
        return await _repository.GetTopRoutesAsync(cancellationToken);
    }
}