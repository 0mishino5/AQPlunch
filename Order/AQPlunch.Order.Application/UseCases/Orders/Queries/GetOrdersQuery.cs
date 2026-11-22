using AQPlunch.OrderService.Domain.Entities;
using AQPlunch.OrderService.Domain.Repositories;
using MediatR;

namespace AQPlunch.OrderService.Application.UseCases.Orders.Queries;

public class GetOrdersQuery : IRequest<List<Order>>
{
}

internal sealed class GetOrdersQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetOrdersQuery, List<Order>>
{
    public async Task<List<Order>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        return await unitOfWork.Orders.ObtenerTodos();
    }
}
