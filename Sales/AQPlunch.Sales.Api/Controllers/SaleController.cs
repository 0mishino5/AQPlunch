using AQPlunch.Sales.Application.UseCases.Sales.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AQPlunch.Sales.Api.Controllers;

[ApiController]
[Authorize(Policy = "Restaurante")]
[Route("api/ventas")]
public class SaleController : ControllerBase
{
    private readonly IMediator _mediator;

    public SaleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize(Policy = "Administrador")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _mediator.Send(new GetSalesQuery()));
    }

    [HttpGet("restaurante/{restaurantId:guid}")]
    public async Task<IActionResult> GetByRestaurant(Guid restaurantId)
    {
        return Ok(await _mediator.Send(new GetSalesByRestaurantQuery { RestaurantId = restaurantId }));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await _mediator.Send(new GetSaleByIdQuery { SaleId = id }));
    }
}
