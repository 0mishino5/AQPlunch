namespace AQPlunch.Identity.Domain.Repositories;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IRoleRepository Roles { get; }

    IRepository<T> Repositorio<T>() where T : class;

    Task<int> GuardarCambios();
}
