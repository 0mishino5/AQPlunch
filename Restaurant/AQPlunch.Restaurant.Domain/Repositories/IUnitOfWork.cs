namespace AQPlunch.RestaurantService.Domain.Repositories;

public interface IUnitOfWork : IDisposable
{
    IDiningTableRepository DiningTables { get; }
    ICategoryRepository Categories { get; }
    IProductRepository Products { get; }

    IRepository<T> Repositorio<T>() where T : class;

    Task<int> GuardarCambios();
}
