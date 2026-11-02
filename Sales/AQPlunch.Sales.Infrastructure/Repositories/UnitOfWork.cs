using System.Collections;
using AQPlunch.Sales.Domain.Repositories;
using AQPlunch.Sales.Infrastructure.Database;

namespace AQPlunch.Sales.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly SalesDbContext _context;
    private readonly Hashtable _repositorios;

    public ISaleRepository Sales { get; }
    public IPaymentMethodRepository PaymentMethods { get; }

    public UnitOfWork(SalesDbContext context)
    {
        _context = context;
        _repositorios = new Hashtable();
        Sales = new SaleRepository(context);
        PaymentMethods = new PaymentMethodRepository(context);
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
