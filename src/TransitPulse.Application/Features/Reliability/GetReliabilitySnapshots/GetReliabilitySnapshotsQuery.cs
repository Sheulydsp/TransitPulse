using MediatR;
using TransitPulse.Application.Features.Reliability.Common;

namespace TransitPulse.Application.Features.Reliability.GetReliabilitySnapshots;

public record GetReliabilitySnapshotsQuery(Guid RouteId)
    : IRequest<List<GetReliabilitySnapshotDto>>;