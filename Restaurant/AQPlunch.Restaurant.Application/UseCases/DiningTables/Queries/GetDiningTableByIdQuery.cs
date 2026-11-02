using AQPlunch.RestaurantService.Application.Security;
using AQPlunch.RestaurantService.Domain.Entities;
using AQPlunch.RestaurantService.Domain.Exceptions;
using AQPlunch.RestaurantService.Domain.Repositories;
using MediatR;

namespace AQPlunch.RestaurantService.Application.UseCases.DiningTables.Queries;

public class GetDiningTableByIdQuery : IRequest<DiningTable>
{
    public Guid TableId { get; set; }
}

internal sealed class GetDiningTableByIdQueryHandler(IUnitOfWork unitOfWork, IUsuarioActual usuarioActual) : IRequestHandler<GetDiningTableByIdQuery, DiningTable>
{
    public async Task<DiningTable> Handle(GetDiningTableByIdQuery request, CancellationToken cancellationToken)
    {
        DiningTable mesa = await unitOfWork.DiningTables.ObtenerPorId(request.TableId) ?? throw new RecursoNoEncontradoException($"No existe la mesa con Id {request.TableId}");

        if (!usuarioActual.PuedeAdministrar(mesa.RestaurantId))
        {
            throw new AccesoDenegadoException("No puedes ver las mesas de otro restaurante");
        }

        return mesa;
    }
}
