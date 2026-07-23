using MediatR;
using TransitPulse.Application.Features.Reliability.Common;

namespace TransitPulse.Application.Features.Reliability.GetReliabilitySnapshot;

public record GetReliabilitySnapshotQuery(Guid SnapshotId)
    : IRequest<GetReliabilitySnapshotDto>;