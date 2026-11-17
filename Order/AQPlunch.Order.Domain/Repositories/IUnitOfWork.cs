namespace AQPlunch.OrderService.Domain.Repositories;

public interface IUnitOfWork : IDisposable
{
    IOrderRepository Orders { get; }

    IRepository<T> Repositorio<T>() where T : class;

    Task<int> GuardarCambios();
}
