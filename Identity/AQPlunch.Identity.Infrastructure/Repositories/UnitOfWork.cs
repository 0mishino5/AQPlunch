using System.Collections;
using AQPlunch.Identity.Domain.Repositories;
using AQPlunch.Identity.Infrastructure.Database;

namespace AQPlunch.Identity.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly IdentityDbContext _context;
    private readonly Hashtable _repositorios;

    public IUserRepository Users { get; }
    public IRoleRepository Roles { get; }

    public UnitOfWork(IdentityDbContext context)
    {
        _context = context;
        _repositorios = new Hashtable();
        Users = new UserRepository(context);
        Roles = new RoleRepository(context);
    }

    public IRepository<T> Repositorio<T>() where T : class
    {
        string tipo = typeof(T).Name;

        if (_repositorios.ContainsKey(tipo))
        {
            return (IRepository<T>)_repositorios[tipo]!;
        }

        Type tipoRepositorio = typeof(GenericRepository<>);
        object? instancia = Activator.CreateInstance(tipoRepositorio.MakeGenericType(typeof(T)), _context);

        if (instancia == null)
        {
            throw new InvalidOperationException($"No se pudo crear el repositorio generico para el tipo {tipo}");
        }

        _repositorios.Add(tipo, instancia);

        return (IRepository<T>)instancia;
    }

    public async Task<int> GuardarCambios()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
