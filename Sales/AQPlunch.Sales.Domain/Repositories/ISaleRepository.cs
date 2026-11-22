using AQPlunch.Sales.Domain.Entities;
using AQPlunch.Sales.Domain.Reports;

namespace AQPlunch.Sales.Domain.Repositories;

public interface ISaleRepository : IRepository<Sale>
{
    Task<List<Sale>> ObtenerPorRestaurante(Guid restaurantId);
    Task<bool> ExistePorPedido(Guid orderId);
    Task<bool> ExistenConMetodoDePago(Guid paymentMethodId);
    Task<List<DailySales>> ObtenerVentasPorDia(Guid restaurantId, DateTime? desde, DateTime? hasta);
    Task<List<PaymentMethodSales>> ObtenerVentasPorMetodoDePago(Guid restaurantId, DateTime? desde, DateTime? hasta);
}
