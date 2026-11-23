using AQPlunch.OrderService.Application.Security;
using AQPlunch.OrderService.Domain.Catalogs;
using AQPlunch.OrderService.Domain.Entities;
using AQPlunch.OrderService.Domain.Exceptions;
using AQPlunch.OrderService.Domain.Repositories;
using MediatR;

namespace AQPlunch.OrderService.Application.UseCases.Orders.Queries;

public class GetOrdersByRestaurantQuery : IRequest<List<Order>>
{
    public Guid RestaurantId { get; set; }
    public string? Estado { get; set; }
}

internal sealed class GetOrdersByRestaurantQueryHandler(IUnitOfWork unitOfWork, IUsuarioActual usuarioActual) : IRequestHandler<GetOrdersByRestaurantQuery, List<Order>>
{
    public async Task<List<Order>> Handle(GetOrdersByRestaurantQuery request, CancellationToken cancellationToken)
    {
        if (!usuarioActual.PuedeAdministrar(request.RestaurantId))
        {
            throw new AccesoDenegadoException("No puedes ver los pedidos de otro restaurante");
        }

        if (string.IsNullOrWhiteSpace(request.Estado))
        {
            return await unitOfWork.Orders.ObtenerPorRestaurante(request.RestaurantId);
        }

        string estado = request.Estado.Trim().ToLowerInvariant();

        if (!EstadoPedido.Existe(estado))
        {
            throw new ReglaNegocioException("El estado debe ser pendiente, en_preparacion, listo, entregado o cancelado");
        }

        return await unitOfWork.Orders.ObtenerPorRestauranteYEstado(request.RestaurantId, estado);
    }
}
