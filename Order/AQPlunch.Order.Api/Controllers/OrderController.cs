using AQPlunch.OrderService.Application.UseCases.Orders.Commands;
using AQPlunch.OrderService.Application.UseCases.Orders.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AQPlunch.OrderService.Api.Controllers;

[ApiController]
[Route("api/pedidos")]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
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
}
