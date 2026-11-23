using AQPlunch.Sales.Domain.Repositories;
using AQPlunch.Sales.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AQPlunch.Sales.Infrastructure.Repositories;

public abstract class Repository<T> : IRepository<T> where T : class
{
    protected readonly SalesDbContext _context;
    protected readonly DbSet<T> _dbSet;

    protected Repository(SalesDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<List<T>> ObtenerTodos()
    {
        return await Consulta().ToListAsync();
    }

    public virtual async Task<T?> ObtenerPorId(Guid id)
    {
        string clave = NombreDeClave();

        return await Consulta()
            .FirstOrDefaultAsync(e => EF.Property<Guid>(e, clave) == id);
    }

    public virtual async Task<bool> Existe(Guid id)
    {
        string clave = NombreDeClave();

        return await _dbSet.AnyAsync(e => EF.Property<Guid>(e, clave) == id);
    }

    public virtual async Task Agregar(T entidad)
    {
        await _dbSet.AddAsync(entidad);
    }

    public virtual void Actualizar(T entidad)
    {
        _dbSet.Update(entidad);
    }

    public virtual void Eliminar(T entidad)
    {
        _dbSet.Remove(entidad);
    }

    protected virtual IQueryable<T> Consulta()
    {
        return _dbSet;
    }

    protected string NombreDeClave()
    {
        return _context.Model
            .FindEntityType(typeof(T))!
            .FindPrimaryKey()!
            .Properties[0]
            .Name;
    }
}
