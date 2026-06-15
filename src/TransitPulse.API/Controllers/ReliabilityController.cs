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
}