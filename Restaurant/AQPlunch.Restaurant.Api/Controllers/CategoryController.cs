using AQPlunch.RestaurantService.Application.UseCases.Categories.Commands;
using AQPlunch.RestaurantService.Application.UseCases.Categories.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AQPlunch.RestaurantService.Api.Controllers;

[ApiController]
[Authorize(Policy = "Restaurante")]
[Route("api/categorias")]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("restaurante/{restaurantId:guid}")]
    public async Task<IActionResult> GetByRestaurant(Guid restaurantId)
    {
        return Ok(await _mediator.Send(new GetCategoriesByRestaurantQuery { RestaurantId = restaurantId }));
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddCategoryCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryCommand command)
    {
        command.CategoryId = id;
        await _mediator.Send(command);

        return Ok(new { message = "Categoria actualizada" });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteCategoryCommand { CategoryId = id });

        return Ok(new { message = "Categoria eliminada" });
    }
}
