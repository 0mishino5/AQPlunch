using AQPlunch.RestaurantService.Domain.Entities;
using AQPlunch.RestaurantService.Domain.Repositories;
using AQPlunch.RestaurantService.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AQPlunch.RestaurantService.Infrastructure.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(RestaurantDbContext context) : base(context)
    {
    }

    public async Task<List<Category>> ObtenerPorRestaurante(Guid restaurantId)
    {
        return await Consulta()
            .Where(c => c.RestaurantId == restaurantId)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<bool> ExisteNombre(Guid restaurantId, string nombre)
    {
        return await _dbSet.AnyAsync(c => c.RestaurantId == restaurantId && c.Name.ToLower() == nombre.ToLower());
    }
}
