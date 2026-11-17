using AQPlunch.Sales.Application.Security;
using AQPlunch.Sales.Domain.Entities;
using AQPlunch.Sales.Domain.Exceptions;
using AQPlunch.Sales.Domain.Repositories;
using MediatR;

namespace AQPlunch.Sales.Application.UseCases.Sales.Queries;

public class GetSalesByRestaurantQuery : IRequest<List<Sale>>
{
    public Guid RestaurantId { get; set; }
}

internal sealed class GetSalesByRestaurantQueryHandler(IUnitOfWork unitOfWork, IUsuarioActual usuarioActual) : IRequestHandler<GetSalesByRestaurantQuery, List<Sale>>
{
    public async Task<List<Sale>> Handle(GetSalesByRestaurantQuery request, CancellationToken cancellationToken)
    {
        if (!usuarioActual.PuedeAdministrar(request.RestaurantId))
        {
            throw new AccesoDenegadoException("No puedes ver las ventas de otro restaurante");
        }

        return await unitOfWork.Sales.ObtenerPorRestaurante(request.RestaurantId);
    }
}
