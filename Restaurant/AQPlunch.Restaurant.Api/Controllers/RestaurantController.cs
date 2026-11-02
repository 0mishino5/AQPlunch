using AQPlunch.RestaurantService.Application.UseCases.Restaurants.Commands;
using AQPlunch.RestaurantService.Application.UseCases.Restaurants.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AQPlunch.RestaurantService.Api.Controllers;

[ApiController]
[Authorize(Policy = "Restaurante")]
[Route("api/restaurantes")]
public class RestaurantController : ControllerBase
{
    private readonly IMediator _mediator;

    public RestaurantController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize(Policy = "Administrador")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _mediator.Send(new GetRestaurantsQuery()));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await _mediator.Send(new GetRestaurantByIdQuery { RestaurantId = id }));
    }

    [Authorize(Policy = "Administrador")]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddRestaurantCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRestaurantCommand command)
    {
        command.RestaurantId = id;
        await _mediator.Send(command);

        return Ok(new { message = "Restaurante actualizado" });
    }

    [Authorize(Policy = "Administrador")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteRestaurantCommand { RestaurantId = id });

        return Ok(new { message = "Restaurante eliminado" });
    }
}
