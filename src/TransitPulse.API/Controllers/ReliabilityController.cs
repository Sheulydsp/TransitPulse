using Microsoft.AspNetCore.Mvc;
using TransitPulse.API.Contracts;
using TransitPulse.Application.Features.Reliability.GenerateReliabilitySnapshot;

namespace TransitPulse.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReliabilityController : ControllerBase
{
    private readonly GenerateReliabilitySnapshotHandler _handler;

    public ReliabilityController(GenerateReliabilitySnapshotHandler handler)
    {
        _handler = handler;
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
            await _handler.HandleAsync(
                command,
                cancellationToken);

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