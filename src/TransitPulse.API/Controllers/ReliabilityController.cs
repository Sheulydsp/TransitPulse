using Microsoft.AspNetCore.Mvc;
using TransitPulse.API.Contracts;
using TransitPulse.Application.Features.Reliability.GenerateReliabilitySnapshot;
using TransitPulse.Application.Features.Reliability.GetReliabilitySnapshots;

namespace TransitPulse.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReliabilityController : ControllerBase
{
    private readonly GenerateReliabilitySnapshotHandler _generateHandler;

    private readonly GetReliabilitySnapshotsHandler _getSnapshotsHandler;

    public ReliabilityController(GenerateReliabilitySnapshotHandler handler, GetReliabilitySnapshotsHandler getSnapshotsHandler)
    {
        _generateHandler = handler;
        _getSnapshotsHandler = getSnapshotsHandler;
    }

    [HttpPost("snapshots/generate")]
    public async Task<IActionResult> GenerateSnapshot(
        GenerateReliabilitySnapshotRequestDTO request,
        CancellationToken cancellationToken)
    {

        if (request.RouteId == Guid.Empty)
        {
            return BadRequest(
                "RouteId is required.");
        }

        if (request.PeriodEnd <= request.PeriodStart)
        {
            return BadRequest(
                "PeriodEnd must be after PeriodStart.");
        }
        var command =
        new GenerateReliabilitySnapshotCommand(
            request.RouteId,
            request.PeriodStart,
            request.PeriodEnd);

        var result =
            await _generateHandler.HandleAsync(
                command,
                cancellationToken);

        return Ok(result);


    }

    [HttpGet("routes/{routeId}/snapshots")]
    public async Task<IActionResult> GetSnapshots(Guid routeId, CancellationToken cancellationToken)
    {
        var result = await _getSnapshotsHandler.HandleAsync(routeId, cancellationToken);

        return Ok(result);
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