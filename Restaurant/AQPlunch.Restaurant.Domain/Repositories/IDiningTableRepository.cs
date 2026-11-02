using AQPlunch.RestaurantService.Domain.Entities;

namespace AQPlunch.RestaurantService.Domain.Repositories;

public interface IDiningTableRepository : IRepository<DiningTable>
{
    Task<DiningTable?> ObtenerPorToken(Guid qrToken);
    Task<List<DiningTable>> ObtenerPorRestaurante(Guid restaurantId);
    Task<bool> ExisteNumero(Guid restaurantId, int tableNumber);
}
