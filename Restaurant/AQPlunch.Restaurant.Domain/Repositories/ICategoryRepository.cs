using AQPlunch.RestaurantService.Domain.Entities;

namespace AQPlunch.RestaurantService.Domain.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<List<Category>> ObtenerPorRestaurante(Guid restaurantId);
    Task<bool> ExisteNombre(Guid restaurantId, string nombre);
}
