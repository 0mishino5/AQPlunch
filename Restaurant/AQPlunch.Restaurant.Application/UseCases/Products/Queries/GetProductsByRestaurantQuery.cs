using AQPlunch.RestaurantService.Application.Security;
using AQPlunch.RestaurantService.Domain.Entities;
using AQPlunch.RestaurantService.Domain.Exceptions;
using AQPlunch.RestaurantService.Domain.Repositories;
using MediatR;

namespace AQPlunch.RestaurantService.Application.UseCases.Products.Queries;

public class GetProductsByRestaurantQuery : IRequest<List<Product>>
{
    public Guid RestaurantId { get; set; }
}

internal sealed class GetProductsByRestaurantQueryHandler(IUnitOfWork unitOfWork, IUsuarioActual usuarioActual) : IRequestHandler<GetProductsByRestaurantQuery, List<Product>>
{
    public async Task<List<Product>> Handle(GetProductsByRestaurantQuery request, CancellationToken cancellationToken)
    {
        if (!usuarioActual.PuedeAdministrar(request.RestaurantId))
        {
            throw new AccesoDenegadoException("No puedes ver los productos de otro restaurante");
        }

        return await unitOfWork.Products.ObtenerPorRestaurante(request.RestaurantId);
    }
}
