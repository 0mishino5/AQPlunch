using System.Collections;
using AQPlunch.OrderService.Domain.Repositories;
using AQPlunch.OrderService.Infrastructure.Database;

namespace AQPlunch.OrderService.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly OrderDbContext _context;
    private readonly Hashtable _repositorios;

    public IOrderRepository Orders { get; }

    public UnitOfWork(OrderDbContext context)
    {
        _context = context;
        _repositorios = new Hashtable();
        Orders = new OrderRepository(context);
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
