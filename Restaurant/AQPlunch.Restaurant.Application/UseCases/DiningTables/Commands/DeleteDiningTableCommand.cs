using AQPlunch.RestaurantService.Application.Security;
using AQPlunch.RestaurantService.Domain.Entities;
using AQPlunch.RestaurantService.Domain.Exceptions;
using AQPlunch.RestaurantService.Domain.Repositories;
using MediatR;

namespace AQPlunch.RestaurantService.Application.UseCases.DiningTables.Commands;

public class DeleteDiningTableCommand : IRequest<Unit>
{
    public Guid TableId { get; set; }
}

internal sealed class DeleteDiningTableCommandHandler(IUnitOfWork unitOfWork, IUsuarioActual usuarioActual) : IRequestHandler<DeleteDiningTableCommand, Unit>
{
    public async Task<Unit> Handle(DeleteDiningTableCommand request, CancellationToken cancellationToken)
    {
        DiningTable mesa = await unitOfWork.DiningTables.ObtenerPorId(request.TableId) ?? throw new RecursoNoEncontradoException($"No existe la mesa con Id {request.TableId}");

        if (!usuarioActual.PuedeAdministrar(mesa.RestaurantId))
        {
            throw new AccesoDenegadoException("No puedes eliminar las mesas de otro restaurante");
        }

        unitOfWork.DiningTables.Eliminar(mesa);
        await unitOfWork.GuardarCambios();

        return Unit.Value;
    }
}
