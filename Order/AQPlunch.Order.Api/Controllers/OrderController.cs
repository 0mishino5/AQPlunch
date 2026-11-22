using AQPlunch.OrderService.Application.UseCases.Orders.Commands;
using AQPlunch.OrderService.Application.UseCases.Orders.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AQPlunch.OrderService.Api.Controllers;

[ApiController]
[Authorize(Policy = "Restaurante")]
[Route("api/pedidos")]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize(Policy = "Administrador")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _mediator.Send(new GetOrdersQuery()));
    }

    [HttpGet("restaurante/{restaurantId:guid}")]
    public async Task<IActionResult> GetByRestaurant(Guid restaurantId, [FromQuery] GetOrdersByRestaurantQuery query)
    {
        query.RestaurantId = restaurantId;

        return Ok(await _mediator.Send(query));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await _mediator.Send(new GetOrderByIdQuery { OrderId = id }));
    }

    [AllowAnonymous]
    [HttpGet("mesa/{qrToken:guid}")]
    public async Task<IActionResult> GetByTableToken(Guid qrToken)
    {
        return Ok(await _mediator.Send(new GetOrdersByTableTokenQuery { QrToken = qrToken }));
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddOrderCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpPut("{id:guid}/estado")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateOrderStatusCommand command)
    {
        command.OrderId = id;
        await _mediator.Send(command);

        return Ok(new { message = "Estado del pedido actualizado" });
    }
}
