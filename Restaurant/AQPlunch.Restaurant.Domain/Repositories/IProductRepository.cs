using AQPlunch.RestaurantService.Domain.Entities;

namespace AQPlunch.RestaurantService.Domain.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<List<Product>> ObtenerPorIds(List<Guid> ids);
    Task<List<Product>> ObtenerPorRestaurante(Guid restaurantId);
    Task<List<Product>> ObtenerDisponiblesPorRestaurante(Guid restaurantId);
    Task<bool> ExistenEnCategoria(Guid categoryId);
}
