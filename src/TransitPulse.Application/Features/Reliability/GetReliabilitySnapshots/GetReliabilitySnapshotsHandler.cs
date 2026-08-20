using MediatR;
using TransitPulse.Application.Exceptions;
using TransitPulse.Application.Features.Reliability.Common;
using TransitPulse.Application.Interfaces;

namespace TransitPulse.Application.Features.Reliability.GetReliabilitySnapshots;

public class GetReliabilitySnapshotsHandler
    : IRequestHandler<
        GetReliabilitySnapshotsQuery,
        List<GetReliabilitySnapshotDto>>
{
    private readonly IReliabilitySnapshotRepository _snapshotRepository;

    public GetReliabilitySnapshotsHandler(
        IReliabilitySnapshotRepository snapshotRepository)
    {
        _snapshotRepository = snapshotRepository;
    }

    public async Task<List<GetReliabilitySnapshotDto>> Handle(
    GetReliabilitySnapshotsQuery query,
    CancellationToken cancellationToken)
    {
        var snapshots = await _snapshotRepository.GetByRouteAsync(
            query.RouteId,
            cancellationToken);

        return snapshots.Select(snapshot =>
            new GetReliabilitySnapshotDto(
                snapshot.Id,
                snapshot.Score,
                snapshot.AverageDelay,
                snapshot.CancellationRate,
                snapshot.OnTimePercentage,
                snapshot.CalculatedAt))
            .ToList();
    }
}
