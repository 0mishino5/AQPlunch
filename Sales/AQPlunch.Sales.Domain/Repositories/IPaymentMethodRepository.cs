using AQPlunch.Sales.Domain.Entities;

namespace AQPlunch.Sales.Domain.Repositories;

public interface IPaymentMethodRepository : IRepository<PaymentMethod>
{
    Task<List<PaymentMethod>> ObtenerActivos();
    Task<PaymentMethod?> ObtenerPorNombre(string nombre);
}
