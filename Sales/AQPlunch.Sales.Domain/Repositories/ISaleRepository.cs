using AQPlunch.Sales.Domain.Entities;

namespace AQPlunch.Sales.Domain.Repositories;

public interface ISaleRepository : IRepository<Sale>
{
    Task<List<Sale>> ObtenerPorRestaurante(Guid restaurantId);
    Task<bool> ExistePorPedido(Guid orderId);
}
