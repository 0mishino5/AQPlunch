using AQPlunch.Sales.Domain.Entities;
using AQPlunch.Sales.Domain.Repositories;
using AQPlunch.Sales.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AQPlunch.Sales.Infrastructure.Repositories;

public class SaleRepository : Repository<Sale>, ISaleRepository
{
    public SaleRepository(SalesDbContext context) : base(context)
    {
    }

    public override async Task<List<Sale>> ObtenerTodos()
    {
        return await Consulta()
            .OrderByDescending(s => s.SoldAt)
            .ToListAsync();
    }

    public async Task<List<Sale>> ObtenerPorRestaurante(Guid restaurantId)
    {
        return await Consulta()
            .Where(s => s.RestaurantId == restaurantId)
            .OrderByDescending(s => s.SoldAt)
            .ToListAsync();
    }

    public async Task<bool> ExistePorPedido(Guid orderId)
    {
        return await _dbSet.AnyAsync(s => s.OrderId == orderId);
    }

    public async Task<bool> ExistenConMetodoDePago(Guid paymentMethodId)
    {
        return await _dbSet.AnyAsync(s => s.PaymentMethodId == paymentMethodId);
    }

    protected override IQueryable<Sale> Consulta()
    {
        return _dbSet
            .Include(s => s.PaymentMethod)
            .Include(s => s.SaleDetails);
    }
}
