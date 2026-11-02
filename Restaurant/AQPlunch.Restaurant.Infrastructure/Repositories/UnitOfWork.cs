using System.Collections;
using AQPlunch.RestaurantService.Domain.Repositories;
using AQPlunch.RestaurantService.Infrastructure.Database;

namespace AQPlunch.RestaurantService.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly RestaurantDbContext _context;
    private readonly Hashtable _repositorios;

    public IDiningTableRepository DiningTables { get; }
    public ICategoryRepository Categories { get; }
    public IProductRepository Products { get; }

    public UnitOfWork(RestaurantDbContext context)
    {
        _context = context;
        _repositorios = new Hashtable();
        DiningTables = new DiningTableRepository(context);
        Categories = new CategoryRepository(context);
        Products = new ProductRepository(context);
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
