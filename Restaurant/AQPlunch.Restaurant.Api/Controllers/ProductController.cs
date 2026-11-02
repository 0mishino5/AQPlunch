using AQPlunch.RestaurantService.Application.UseCases.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AQPlunch.RestaurantService.Api.Controllers;

[ApiController]
[Route("api/productos")]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpGet("por-ids")]
    public async Task<IActionResult> GetByIds([FromQuery] GetProductsByIdsQuery query)
    {
        return Ok(await _mediator.Send(query));
    }
}
