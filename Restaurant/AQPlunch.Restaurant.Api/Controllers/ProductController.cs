using AQPlunch.RestaurantService.Application.UseCases.Products.Commands;
using AQPlunch.RestaurantService.Application.UseCases.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AQPlunch.RestaurantService.Api.Controllers;

[ApiController]
[Authorize(Policy = "Restaurante")]
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

    [AllowAnonymous]
    [HttpGet("menu/{restaurantId:guid}")]
    public async Task<IActionResult> GetMenu(Guid restaurantId)
    {
        return Ok(await _mediator.Send(new GetMenuQuery { RestaurantId = restaurantId }));
    }

    [HttpGet("restaurante/{restaurantId:guid}")]
    public async Task<IActionResult> GetByRestaurant(Guid restaurantId)
    {
        return Ok(await _mediator.Send(new GetProductsByRestaurantQuery { RestaurantId = restaurantId }));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await _mediator.Send(new GetProductByIdQuery { ProductId = id }));
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddProductCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductCommand command)
    {
        command.ProductId = id;
        await _mediator.Send(command);

        return Ok(new { message = "Producto actualizado" });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteProductCommand { ProductId = id });

        return Ok(new { message = "Producto eliminado" });
    }
}
