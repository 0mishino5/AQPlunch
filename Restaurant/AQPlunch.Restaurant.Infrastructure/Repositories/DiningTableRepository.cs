using AQPlunch.RestaurantService.Domain.Entities;
using AQPlunch.RestaurantService.Domain.Repositories;
using AQPlunch.RestaurantService.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AQPlunch.RestaurantService.Infrastructure.Repositories;

public class DiningTableRepository : Repository<DiningTable>, IDiningTableRepository
{
    public DiningTableRepository(RestaurantDbContext context) : base(context)
    {
    }

    public async Task<DiningTable?> ObtenerPorToken(Guid qrToken)
    {
        return await Consulta()
            .FirstOrDefaultAsync(t => t.QrToken == qrToken);
    }

    public async Task<List<DiningTable>> ObtenerPorRestaurante(Guid restaurantId)
    {
        return await Consulta()
            .Where(t => t.RestaurantId == restaurantId)
            .OrderBy(t => t.TableNumber)
            .ToListAsync();
    }

    public async Task<bool> ExisteNumero(Guid restaurantId, int tableNumber)
    {
        return await _dbSet.AnyAsync(t => t.RestaurantId == restaurantId && t.TableNumber == tableNumber);
    }

    protected override IQueryable<DiningTable> Consulta()
    {
        return _dbSet.Include(t => t.Restaurant);
    }
}
