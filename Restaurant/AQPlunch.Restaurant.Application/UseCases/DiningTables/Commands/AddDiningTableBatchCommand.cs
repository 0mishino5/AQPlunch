using AQPlunch.RestaurantService.Application.Security;
using AQPlunch.RestaurantService.Domain.Entities;
using AQPlunch.RestaurantService.Domain.Exceptions;
using AQPlunch.RestaurantService.Domain.Repositories;
using MediatR;

namespace AQPlunch.RestaurantService.Application.UseCases.DiningTables.Commands;

public class AddDiningTableBatchCommand : IRequest<List<Guid>>
{
    public Guid RestaurantId { get; set; }
    public int Quantity { get; set; }
}

internal sealed class AddDiningTableBatchCommandHandler(IUnitOfWork unitOfWork, IUsuarioActual usuarioActual) : IRequestHandler<AddDiningTableBatchCommand, List<Guid>>
{
    public async Task<List<Guid>> Handle(AddDiningTableBatchCommand request, CancellationToken cancellationToken)
    {
        if (!usuarioActual.PuedeAdministrar(request.RestaurantId))
        {
            throw new AccesoDenegadoException("No puedes agregar mesas a otro restaurante");
        }

        if (!await unitOfWork.Repositorio<Restaurant>().Existe(request.RestaurantId))
        {
            throw new RecursoNoEncontradoException($"No existe el restaurante con Id {request.RestaurantId}");
        }

        if (request.Quantity < 1 || request.Quantity > 50)
        {
            throw new ReglaNegocioException("La cantidad de mesas debe estar entre 1 y 50");
        }

        int ultimoNumero = await unitOfWork.DiningTables.ObtenerUltimoNumero(request.RestaurantId);

        if (ultimoNumero > int.MaxValue - request.Quantity)
        {
            throw new ReglaNegocioException("El numero de mesa supera el maximo permitido");
        }

        List<Guid> ids = new List<Guid>();

        for (int i = 1; i <= request.Quantity; i++)
        {
            DiningTable mesa = new DiningTable
            {
                TableId = Guid.NewGuid(),
                RestaurantId = request.RestaurantId,
                TableNumber = ultimoNumero + i,
                QrToken = Guid.NewGuid(),
                IsActive = true
            };

            await unitOfWork.DiningTables.Agregar(mesa);
            ids.Add(mesa.TableId);
        }

        await unitOfWork.GuardarCambios();

        return ids;
    }
}
