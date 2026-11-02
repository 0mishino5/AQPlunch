using AQPlunch.RestaurantService.Domain.Entities;
using AQPlunch.RestaurantService.Domain.Repositories;
using AQPlunch.RestaurantService.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AQPlunch.RestaurantService.Infrastructure.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(RestaurantDbContext context) : base(context)
    {
    }

    public async Task<List<Product>> ObtenerPorIds(List<Guid> ids)
    {
        return await Consulta()
            .Where(p => ids.Contains(p.ProductId))
            .ToListAsync();
    }

    public async Task<List<Product>> ObtenerPorRestaurante(Guid restaurantId)
    {
        return await Consulta()
            .Where(p => p.Category.RestaurantId == restaurantId)
            .OrderBy(p => p.Category.Name)
            .ThenBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<List<Product>> ObtenerDisponiblesPorRestaurante(Guid restaurantId)
    {
        return await Consulta()
            .Where(p => p.Category.RestaurantId == restaurantId && p.IsAvailable)
            .OrderBy(p => p.Category.Name)
            .ThenBy(p => p.Name)
            .ToListAsync();
    }

    protected override IQueryable<Product> Consulta()
    {
        return _dbSet.Include(p => p.Category);
    }
}
