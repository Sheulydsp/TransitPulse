using MediatR;
using Microsoft.AspNetCore.Mvc;
using TransitPulse.Application.Features.Dashboard.GetTopRoutes;

namespace TransitPulse.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public DashboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("top-routes")]
    public async Task<ActionResult<List<TopRouteDto>>> GetTopRoutes(
    CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTopRoutesQuery(), cancellationToken);

        return Ok(result);
    }
}