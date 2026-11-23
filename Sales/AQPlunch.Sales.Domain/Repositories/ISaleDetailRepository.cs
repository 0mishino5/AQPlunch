using AQPlunch.Sales.Domain.Entities;
using AQPlunch.Sales.Domain.Reports;

namespace AQPlunch.Sales.Domain.Repositories;

public interface ISaleDetailRepository : IRepository<SaleDetail>
{
    Task<List<ProductSales>> ObtenerProductosMasVendidos(Guid restaurantId, DateTime? desde, DateTime? hasta, int top);
}
