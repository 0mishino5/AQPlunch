using AQPlunch.RestaurantService.Application.Security;
using AQPlunch.RestaurantService.Domain.Entities;
using AQPlunch.RestaurantService.Domain.Exceptions;
using AQPlunch.RestaurantService.Domain.Repositories;
using MediatR;

namespace AQPlunch.RestaurantService.Application.UseCases.DiningTables.Commands;

public class UpdateDiningTableCommand : IRequest<Unit>
{
    public Guid TableId { get; set; }
    public int TableNumber { get; set; }
    public bool IsActive { get; set; }
}

internal sealed class UpdateDiningTableCommandHandler(IUnitOfWork unitOfWork, IUsuarioActual usuarioActual) : IRequestHandler<UpdateDiningTableCommand, Unit>
{
    public async Task<Unit> Handle(UpdateDiningTableCommand request, CancellationToken cancellationToken)
    {
        DiningTable mesa = await unitOfWork.DiningTables.ObtenerPorId(request.TableId) ?? throw new RecursoNoEncontradoException($"No existe la mesa con Id {request.TableId}");

        if (!usuarioActual.PuedeAdministrar(mesa.RestaurantId))
        {
            throw new AccesoDenegadoException("No puedes modificar las mesas de otro restaurante");
        }

        if (request.TableNumber <= 0)
        {
            throw new ReglaNegocioException("El numero de mesa debe ser mayor que 0");
        }

        if (request.TableNumber != mesa.TableNumber && await unitOfWork.DiningTables.ExisteNumero(mesa.RestaurantId, request.TableNumber))
        {
            throw new ReglaNegocioException($"Ya existe la mesa {request.TableNumber} en este restaurante");
        }

        mesa.TableNumber = request.TableNumber;
        mesa.IsActive = request.IsActive;

        unitOfWork.DiningTables.Actualizar(mesa);
        await unitOfWork.GuardarCambios();

        return Unit.Value;
    }
}
