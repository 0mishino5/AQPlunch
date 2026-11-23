using AQPlunch.RestaurantService.Application.Security;
using AQPlunch.RestaurantService.Domain.Entities;
using AQPlunch.RestaurantService.Domain.Exceptions;
using AQPlunch.RestaurantService.Domain.Repositories;
using MediatR;

namespace AQPlunch.RestaurantService.Application.UseCases.DiningTables.Commands;

public class RegenerateTableQrCommand : IRequest<Guid>
{
    public Guid TableId { get; set; }
}

internal sealed class RegenerateTableQrCommandHandler(IUnitOfWork unitOfWork, IUsuarioActual usuarioActual) : IRequestHandler<RegenerateTableQrCommand, Guid>
{
    public async Task<Guid> Handle(RegenerateTableQrCommand request, CancellationToken cancellationToken)
    {
        DiningTable mesa = await unitOfWork.DiningTables.ObtenerPorId(request.TableId) ?? throw new RecursoNoEncontradoException($"No existe la mesa con Id {request.TableId}");

        if (!usuarioActual.PuedeAdministrar(mesa.RestaurantId))
        {
            throw new AccesoDenegadoException("No puedes modificar las mesas de otro restaurante");
        }

        mesa.QrToken = Guid.NewGuid();

        unitOfWork.DiningTables.Actualizar(mesa);
        await unitOfWork.GuardarCambios();

        return mesa.QrToken;
    }
}
