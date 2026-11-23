using AQPlunch.OrderService.Application.Clients;
using AQPlunch.OrderService.Domain.Entities;
using AQPlunch.OrderService.Domain.Repositories;
using MediatR;

namespace AQPlunch.OrderService.Application.UseCases.Orders.Queries;

public class GetOrdersByTableTokenQuery : IRequest<List<Order>>
{
    public Guid QrToken { get; set; }
}

internal sealed class GetOrdersByTableTokenQueryHandler(IUnitOfWork unitOfWork, IRestaurantClient restaurantClient) : IRequestHandler<GetOrdersByTableTokenQuery, List<Order>>
{
    public async Task<List<Order>> Handle(GetOrdersByTableTokenQuery request, CancellationToken cancellationToken)
    {
        TableInfo mesa = await restaurantClient.ObtenerMesaPorToken(request.QrToken);

        return await unitOfWork.Orders.ObtenerActivosPorMesa(mesa.TableId);
    }
}
