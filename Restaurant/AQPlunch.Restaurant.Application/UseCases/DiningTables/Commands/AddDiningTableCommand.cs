using AQPlunch.RestaurantService.Application.Security;
using AQPlunch.RestaurantService.Domain.Entities;
using AQPlunch.RestaurantService.Domain.Exceptions;
using AQPlunch.RestaurantService.Domain.Repositories;
using MediatR;

namespace AQPlunch.RestaurantService.Application.UseCases.DiningTables.Commands;

public class AddDiningTableCommand : IRequest<Guid>
{
    public Guid RestaurantId { get; set; }
    public int TableNumber { get; set; }
}

internal sealed class AddDiningTableCommandHandler(IUnitOfWork unitOfWork, IUsuarioActual usuarioActual) : IRequestHandler<AddDiningTableCommand, Guid>
{
    public async Task<Guid> Handle(AddDiningTableCommand request, CancellationToken cancellationToken)
    {
        if (!usuarioActual.PuedeAdministrar(request.RestaurantId))
        {
            throw new AccesoDenegadoException("No puedes agregar mesas a otro restaurante");
        }

        if (!await unitOfWork.Repositorio<Restaurant>().Existe(request.RestaurantId))
        {
            throw new RecursoNoEncontradoException($"No existe el restaurante con Id {request.RestaurantId}");
        }

        if (request.TableNumber <= 0)
        {
            throw new ReglaNegocioException("El numero de mesa debe ser mayor que 0");
        }

        if (await unitOfWork.DiningTables.ExisteNumero(request.RestaurantId, request.TableNumber))
        {
            throw new ReglaNegocioException($"Ya existe la mesa {request.TableNumber} en este restaurante");
        }

        DiningTable mesa = new DiningTable
        {
            TableId = Guid.NewGuid(),
            RestaurantId = request.RestaurantId,
            TableNumber = request.TableNumber,
            QrToken = Guid.NewGuid(),
            IsActive = true
        };

        await unitOfWork.DiningTables.Agregar(mesa);
        await unitOfWork.GuardarCambios();

        return mesa.TableId;
    }
}
