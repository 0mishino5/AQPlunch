using AQPlunch.Sales.Application.UseCases.Reports.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AQPlunch.Sales.Api.Controllers;

[ApiController]
[Authorize(Policy = "Restaurante")]
[Route("api/reportes/restaurante/{restaurantId:guid}")]
public class ReportController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("ventas-por-dia")]
    public async Task<IActionResult> GetDailySales(Guid restaurantId, [FromQuery] GetDailySalesQuery query)
    {
        query.RestaurantId = restaurantId;

        return Ok(await _mediator.Send(query));
    }

    [HttpGet("productos-mas-vendidos")]
    public async Task<IActionResult> GetTopProducts(Guid restaurantId, [FromQuery] GetTopProductsQuery query)
    {
        query.RestaurantId = restaurantId;

        return Ok(await _mediator.Send(query));
    }

    [HttpGet("metodos-de-pago")]
    public async Task<IActionResult> GetSalesByPaymentMethod(Guid restaurantId, [FromQuery] GetSalesByPaymentMethodQuery query)
    {
        query.RestaurantId = restaurantId;

        return Ok(await _mediator.Send(query));
    }
}
