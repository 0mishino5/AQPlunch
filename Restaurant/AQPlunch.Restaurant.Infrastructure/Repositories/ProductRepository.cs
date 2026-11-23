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

    public async Task<List<Product>> Buscar(Guid restaurantId, string? texto, Guid? categoryId, bool soloDisponibles)
    {
        IQueryable<Product> consulta = Consulta().Where(p => p.Category.RestaurantId == restaurantId);

        if (!string.IsNullOrWhiteSpace(texto))
        {
            string buscado = texto.Trim().ToLower();

            consulta = consulta.Where(p => p.Name.ToLower().Contains(buscado) || (p.Description != null && p.Description.ToLower().Contains(buscado)));
        }

        if (categoryId != null)
        {
            consulta = consulta.Where(p => p.CategoryId == categoryId);
        }

        if (soloDisponibles)
        {
            consulta = consulta.Where(p => p.IsAvailable);
        }

        return await consulta
            .OrderBy(p => p.Category.Name)
            .ThenBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<bool> ExistenEnCategoria(Guid categoryId)
    {
        return await _dbSet.AnyAsync(p => p.CategoryId == categoryId);
    }

    protected override IQueryable<Product> Consulta()
    {
        return _dbSet.Include(p => p.Category);
    }
}
