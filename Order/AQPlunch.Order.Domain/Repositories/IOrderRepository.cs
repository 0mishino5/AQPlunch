using AQPlunch.OrderService.Domain.Entities;

namespace AQPlunch.OrderService.Domain.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    Task<List<Order>> ObtenerPorRestaurante(Guid restaurantId);
    Task<List<Order>> ObtenerPorRestauranteYEstado(Guid restaurantId, string estado);
    Task<List<Order>> ObtenerPorMesa(Guid tableId);
}
