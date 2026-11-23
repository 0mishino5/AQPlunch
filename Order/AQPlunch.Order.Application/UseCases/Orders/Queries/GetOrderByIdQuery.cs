using AQPlunch.OrderService.Application.Security;
using AQPlunch.OrderService.Domain.Entities;
using AQPlunch.OrderService.Domain.Exceptions;
using AQPlunch.OrderService.Domain.Repositories;
using MediatR;

namespace AQPlunch.OrderService.Application.UseCases.Orders.Queries;

public class GetOrderByIdQuery : IRequest<Order>
{
    public Guid OrderId { get; set; }
}

internal sealed class GetOrderByIdQueryHandler(IUnitOfWork unitOfWork, IUsuarioActual usuarioActual) : IRequestHandler<GetOrderByIdQuery, Order>
{
    public async Task<Order> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        Order pedido = await unitOfWork.Orders.ObtenerPorId(request.OrderId) ?? throw new RecursoNoEncontradoException($"No existe el pedido con Id {request.OrderId}");

        if (!usuarioActual.PuedeAdministrar(pedido.RestaurantId))
        {
            throw new AccesoDenegadoException("No puedes ver los pedidos de otro restaurante");
        }

        return pedido;
    }
}
