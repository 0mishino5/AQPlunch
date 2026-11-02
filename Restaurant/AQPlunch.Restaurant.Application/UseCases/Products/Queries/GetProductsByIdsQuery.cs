using AQPlunch.RestaurantService.Application.Results;
using AQPlunch.RestaurantService.Domain.Entities;
using AQPlunch.RestaurantService.Domain.Exceptions;
using AQPlunch.RestaurantService.Domain.Repositories;
using MediatR;

namespace AQPlunch.RestaurantService.Application.UseCases.Products.Queries;

public class GetProductsByIdsQuery : IRequest<List<ProductPriceResult>>
{
    public List<Guid> Ids { get; set; } = new List<Guid>();
}

internal sealed class GetProductsByIdsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetProductsByIdsQuery, List<ProductPriceResult>>
{
    public async Task<List<ProductPriceResult>> Handle(GetProductsByIdsQuery request, CancellationToken cancellationToken)
    {
        if (request.Ids.Count == 0)
        {
            throw new ReglaNegocioException("Envia al menos un id de producto");
        }

        List<Product> productos = await unitOfWork.Products.ObtenerPorIds(request.Ids);

        return productos.Select(p => new ProductPriceResult
        {
            ProductId = p.ProductId,
            Name = p.Name,
            Price = p.Price,
            IsAvailable = p.IsAvailable,
            RestaurantId = p.Category.RestaurantId
        }).ToList();
    }
}
