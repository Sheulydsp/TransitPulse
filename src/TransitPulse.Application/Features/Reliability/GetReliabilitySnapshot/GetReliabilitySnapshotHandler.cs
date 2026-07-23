using MediatR;
using TransitPulse.Application.Exceptions;
using TransitPulse.Application.Features.Reliability.Common;
using TransitPulse.Application.Interfaces;

namespace TransitPulse.Application.Features.Reliability.GetReliabilitySnapshot;

public class GetReliabilitySnapshotHandler
    : IRequestHandler<
        GetReliabilitySnapshotQuery,
        GetReliabilitySnapshotDto>
{
    private readonly IReliabilitySnapshotRepository _snapshotRepository;

    public GetReliabilitySnapshotHandler(
        IReliabilitySnapshotRepository snapshotRepository)
    {
        _snapshotRepository = snapshotRepository;
    }

    public async Task<GetReliabilitySnapshotDto> Handle(
        GetReliabilitySnapshotQuery query,
        CancellationToken cancellationToken)
    {
        var snapshot = await _snapshotRepository.GetByIdAsync(
            query.SnapshotId,
            cancellationToken);

        if (snapshot is null)
        {
            throw new NotFoundException(
                $"Snapshot {query.SnapshotId} was not found.");
        }

        return new GetReliabilitySnapshotDto(
            snapshot.Id,
            snapshot.Score,
            snapshot.AverageDelay,
            snapshot.CancellationRate,
            snapshot.OnTimePercentage,
            snapshot.CalculatedAt);
    }
}