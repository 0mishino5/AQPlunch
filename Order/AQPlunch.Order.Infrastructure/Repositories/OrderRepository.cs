using AQPlunch.OrderService.Domain.Entities;
using AQPlunch.OrderService.Domain.Repositories;
using AQPlunch.OrderService.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AQPlunch.OrderService.Infrastructure.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(OrderDbContext context) : base(context)
    {
    }

    public override async Task<List<Order>> ObtenerTodos()
    {
        return await Consulta()
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Order>> ObtenerPorRestaurante(Guid restaurantId)
    {
        return await Consulta()
            .Where(o => o.RestaurantId == restaurantId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Order>> ObtenerPorRestauranteYEstado(Guid restaurantId, string estado)
    {
        return await Consulta()
            .Where(o => o.RestaurantId == restaurantId && o.Status == estado)
            .OrderBy(o => o.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Order>> ObtenerPorMesa(Guid tableId)
    {
        return await Consulta()
            .Where(o => o.TableId == tableId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
    }

    protected override IQueryable<Order> Consulta()
    {
        return _dbSet.Include(o => o.OrderDetails);
    }
}
