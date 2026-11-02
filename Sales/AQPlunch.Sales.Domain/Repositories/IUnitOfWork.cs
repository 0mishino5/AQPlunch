namespace AQPlunch.Sales.Domain.Repositories;

public interface IUnitOfWork : IDisposable
{
    ISaleRepository Sales { get; }
    IPaymentMethodRepository PaymentMethods { get; }

    IRepository<T> Repositorio<T>() where T : class;

    Task<int> GuardarCambios();
}
