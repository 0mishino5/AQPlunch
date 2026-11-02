using AQPlunch.RestaurantService.Application.Results;
using AQPlunch.RestaurantService.Domain.Entities;
using AQPlunch.RestaurantService.Domain.Exceptions;
using AQPlunch.RestaurantService.Domain.Repositories;
using MediatR;

namespace AQPlunch.RestaurantService.Application.UseCases.DiningTables.Queries;

public class GetDiningTableByTokenQuery : IRequest<TableByTokenResult>
{
    public Guid QrToken { get; set; }
}

internal sealed class GetDiningTableByTokenQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetDiningTableByTokenQuery, TableByTokenResult>
{
    public async Task<TableByTokenResult> Handle(GetDiningTableByTokenQuery request, CancellationToken cancellationToken)
    {
        DiningTable mesa = await unitOfWork.DiningTables.ObtenerPorToken(request.QrToken) ?? throw new RecursoNoEncontradoException("No existe una mesa con ese codigo QR");

        if (!mesa.IsActive || !mesa.Restaurant.IsActive)
        {
            throw new ReglaNegocioException("La mesa no esta disponible para recibir pedidos");
        }

        return new TableByTokenResult
        {
            TableId = mesa.TableId,
            TableNumber = mesa.TableNumber,
            RestaurantId = mesa.RestaurantId,
            RestaurantName = mesa.Restaurant.Name
        };
    }
}
