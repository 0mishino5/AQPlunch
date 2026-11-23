using AQPlunch.Sales.Domain.Entities;
using AQPlunch.Sales.Domain.Reports;
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

    public async Task<List<DailySales>> ObtenerVentasPorDia(Guid restaurantId, DateTime? desde, DateTime? hasta)
    {
        return await FiltrarPorRestauranteYFechas(restaurantId, desde, hasta)
            .GroupBy(s => s.SoldAt!.Value.Date)
            .Select(g => new DailySales
            {
                Date = g.Key,
                SalesCount = g.Count(),
                Total = g.Sum(s => s.Total)
            })
            .OrderBy(r => r.Date)
            .ToListAsync();
    }

    public async Task<List<PaymentMethodSales>> ObtenerVentasPorMetodoDePago(Guid restaurantId, DateTime? desde, DateTime? hasta)
    {
        return await FiltrarPorRestauranteYFechas(restaurantId, desde, hasta)
            .GroupBy(s => new { s.PaymentMethodId, s.PaymentMethod.Name })
            .Select(g => new PaymentMethodSales
            {
                PaymentMethodId = g.Key.PaymentMethodId,
                PaymentMethodName = g.Key.Name,
                SalesCount = g.Count(),
                Total = g.Sum(s => s.Total)
            })
            .OrderByDescending(r => r.Total)
            .ToListAsync();
    }

    protected override IQueryable<Sale> Consulta()
    {
        return _dbSet
            .Include(s => s.PaymentMethod)
            .Include(s => s.SaleDetails);
    }

    private IQueryable<Sale> FiltrarPorRestauranteYFechas(Guid restaurantId, DateTime? desde, DateTime? hasta)
    {
        IQueryable<Sale> ventas = _dbSet.Where(s => s.RestaurantId == restaurantId);

        if (desde != null)
        {
            ventas = ventas.Where(s => s.SoldAt >= desde.Value.Date);
        }

        if (hasta != null)
        {
            ventas = ventas.Where(s => s.SoldAt < hasta.Value.Date.AddDays(1));
        }

        return ventas;
    }
}
