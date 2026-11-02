using AQPlunch.RestaurantService.Application.Security;
using AQPlunch.RestaurantService.Domain.Entities;
using AQPlunch.RestaurantService.Domain.Exceptions;
using AQPlunch.RestaurantService.Domain.Repositories;
using MediatR;

namespace AQPlunch.RestaurantService.Application.UseCases.Products.Queries;

public class GetProductByIdQuery : IRequest<Product>
{
    public Guid ProductId { get; set; }
}

internal sealed class GetProductByIdQueryHandler(IUnitOfWork unitOfWork, IUsuarioActual usuarioActual) : IRequestHandler<GetProductByIdQuery, Product>
{
    public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        Product producto = await unitOfWork.Products.ObtenerPorId(request.ProductId) ?? throw new RecursoNoEncontradoException($"No existe el producto con Id {request.ProductId}");

        if (!usuarioActual.PuedeAdministrar(producto.Category.RestaurantId))
        {
            throw new AccesoDenegadoException("No puedes ver los productos de otro restaurante");
        }

        return producto;
    }
}
