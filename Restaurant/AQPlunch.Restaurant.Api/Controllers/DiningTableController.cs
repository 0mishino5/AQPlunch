using AQPlunch.RestaurantService.Application.UseCases.DiningTables.Commands;
using AQPlunch.RestaurantService.Application.UseCases.DiningTables.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AQPlunch.RestaurantService.Api.Controllers;

[ApiController]
[Authorize(Policy = "Restaurante")]
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

    [HttpGet("restaurante/{restaurantId:guid}")]
    public async Task<IActionResult> GetByRestaurant(Guid restaurantId)
    {
        return Ok(await _mediator.Send(new GetDiningTablesByRestaurantQuery { RestaurantId = restaurantId }));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await _mediator.Send(new GetDiningTableByIdQuery { TableId = id }));
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddDiningTableCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDiningTableCommand command)
    {
        command.TableId = id;
        await _mediator.Send(command);

        return Ok(new { message = "Mesa actualizada" });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteDiningTableCommand { TableId = id });

        return Ok(new { message = "Mesa eliminada" });
    }
}
