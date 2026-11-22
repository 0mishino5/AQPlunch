using AQPlunch.Sales.Domain.Entities;
using AQPlunch.Sales.Domain.Reports;
using AQPlunch.Sales.Domain.Repositories;
using AQPlunch.Sales.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AQPlunch.Sales.Infrastructure.Repositories;

public class SaleDetailRepository : Repository<SaleDetail>, ISaleDetailRepository
{
    public SaleDetailRepository(SalesDbContext context) : base(context)
    {
    }

    public async Task<List<ProductSales>> ObtenerProductosMasVendidos(Guid restaurantId, DateTime? desde, DateTime? hasta, int top)
    {
        IQueryable<SaleDetail> detalles = _dbSet.Where(d => d.Sale.RestaurantId == restaurantId);

        if (desde != null)
        {
            detalles = detalles.Where(d => d.Sale.SoldAt >= desde.Value.Date);
        }

        if (hasta != null)
        {
            detalles = detalles.Where(d => d.Sale.SoldAt < hasta.Value.Date.AddDays(1));
        }

        return await detalles
            .GroupBy(d => new { d.ProductId, d.ProductName })
            .Select(g => new ProductSales
            {
                ProductId = g.Key.ProductId,
                ProductName = g.Key.ProductName,
                QuantitySold = g.Sum(d => d.Quantity),
                Total = g.Sum(d => d.Subtotal)
            })
            .OrderByDescending(r => r.QuantitySold)
            .ThenByDescending(r => r.Total)
            .Take(top)
            .ToListAsync();
    }
}
