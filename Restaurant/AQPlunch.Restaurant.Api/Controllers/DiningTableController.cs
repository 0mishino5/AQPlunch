using AQPlunch.RestaurantService.Application.UseCases.DiningTables.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AQPlunch.RestaurantService.Api.Controllers;

[ApiController]
[Route("api/mesas")]
public class DiningTableController : ControllerBase
{
    private readonly IMediator _mediator;

    public DiningTableController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpGet("token/{token:guid}")]
    public async Task<IActionResult> GetByToken(Guid token)
    {
        return Ok(await _mediator.Send(new GetDiningTableByTokenQuery { QrToken = token }));
    }
}
