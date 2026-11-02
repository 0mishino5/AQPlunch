using AQPlunch.RestaurantService.Application.Security;
using AQPlunch.RestaurantService.Domain.Entities;
using AQPlunch.RestaurantService.Domain.Exceptions;
using AQPlunch.RestaurantService.Domain.Repositories;
using MediatR;

namespace AQPlunch.RestaurantService.Application.UseCases.DiningTables.Queries;

public class GetDiningTablesByRestaurantQuery : IRequest<List<DiningTable>>
{
    public Guid RestaurantId { get; set; }
}

internal sealed class GetDiningTablesByRestaurantQueryHandler(IUnitOfWork unitOfWork, IUsuarioActual usuarioActual) : IRequestHandler<GetDiningTablesByRestaurantQuery, List<DiningTable>>
{
    public async Task<List<DiningTable>> Handle(GetDiningTablesByRestaurantQuery request, CancellationToken cancellationToken)
    {
        if (!usuarioActual.PuedeAdministrar(request.RestaurantId))
        {
            throw new AccesoDenegadoException("No puedes ver las mesas de otro restaurante");
        }

        return await unitOfWork.DiningTables.ObtenerPorRestaurante(request.RestaurantId);
    }
}
