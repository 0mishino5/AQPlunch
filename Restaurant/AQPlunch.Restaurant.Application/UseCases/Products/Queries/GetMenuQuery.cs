using AQPlunch.RestaurantService.Application.Results;
using AQPlunch.RestaurantService.Domain.Entities;
using AQPlunch.RestaurantService.Domain.Exceptions;
using AQPlunch.RestaurantService.Domain.Repositories;
using MediatR;

namespace AQPlunch.RestaurantService.Application.UseCases.Products.Queries;

public class GetMenuQuery : IRequest<List<MenuCategoryResult>>
{
    public Guid RestaurantId { get; set; }
}

internal sealed class GetMenuQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetMenuQuery, List<MenuCategoryResult>>
{
    public async Task<List<MenuCategoryResult>> Handle(GetMenuQuery request, CancellationToken cancellationToken)
    {
        Restaurant restaurante = await unitOfWork.Repositorio<Restaurant>().ObtenerPorId(request.RestaurantId) ?? throw new RecursoNoEncontradoException($"No existe el restaurante con Id {request.RestaurantId}");

        if (!restaurante.IsActive)
        {
            throw new ReglaNegocioException("El restaurante no esta disponible");
        }

        List<Product> productos = await unitOfWork.Products.ObtenerDisponiblesPorRestaurante(request.RestaurantId);

        return productos
            .GroupBy(p => p.Category)
            .Select(g => new MenuCategoryResult
            {
                CategoryId = g.Key.CategoryId,
                CategoryName = g.Key.Name,
                Products = g.Select(p => new MenuProductResult
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price
                }).ToList()
            })
            .ToList();
    }
}
