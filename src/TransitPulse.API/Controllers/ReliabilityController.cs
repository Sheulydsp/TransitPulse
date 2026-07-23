using MediatR;
using Microsoft.AspNetCore.Mvc;
using TransitPulse.API.Contracts.Requests;
using TransitPulse.API.Contracts.Responses;
using TransitPulse.Application.Features.Reliability.GenerateReliabilitySnapshot;
using TransitPulse.Application.Features.Reliability.GetReliabilitySnapshot;
using TransitPulse.Application.Features.Reliability.GetReliabilitySnapshots;

namespace TransitPulse.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReliabilityController : ControllerBase
{

    private readonly IMediator _mediator;

    public ReliabilityController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("snapshots")]
    public async Task<IActionResult> GenerateSnapshot(GenerateReliabilitySnapshotRequest request,
        CancellationToken cancellationToken)
    {
        var command = new GenerateReliabilitySnapshotCommand(
            request.RouteId,
            request.PeriodStart,
            request.PeriodEnd);

        var result = await _mediator.Send(command, cancellationToken);

        var response = new GenerateReliabilitySnapshotResponse(

            result.SnapshotId,
            result.Score,
            result.AverageDelay,
            result.CancellationRate);

        return CreatedAtAction(
            nameof(GetSnapshots),
            new { routeId = request.RouteId },
            response);

        //return Ok(result);
    }

    [HttpGet("routes/{routeId}/snapshots")]
    public async Task<IActionResult> GetSnapshots(Guid routeId, CancellationToken cancellationToken)
    {
        var snapshots = await _mediator.Send(new GetReliabilitySnapshotsQuery(routeId), cancellationToken);

        var response = snapshots.Select(snapshot =>
            new GetReliabilitySnapshotResponse(
                snapshot.SnapshotId,
                snapshot.Score,
                snapshot.AverageDelay,
                snapshot.CancellationRate,
                snapshot.OnTimePercentage,
                snapshot.CalculatedAt))
            .ToList();

        return Ok(response);
    }

    [HttpGet("snapshots/{snapshotId}")]

    public async Task<IActionResult> Getsnapshot(Guid snapshotId,
    CancellationToken cancellationToken)
    {
        var snapshot = await _mediator.Send(new GetReliabilitySnapshotQuery(snapshotId),
    cancellationToken);

        var response = new GetReliabilitySnapshotResponse(
                        snapshot.SnapshotId,
                        snapshot.Score,
                        snapshot.AverageDelay,
                        snapshot.CancellationRate,
                        snapshot.OnTimePercentage,
                        snapshot.CalculatedAt);

        return Ok(response);




    }

    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok("TransitPulse API is running");
    }

    // test exception
    /*[HttpGet("test-error")]
    public IActionResult TestError()
    {
        throw new Exception(
        "Test exception from TransitPulse.");
    }*/
}