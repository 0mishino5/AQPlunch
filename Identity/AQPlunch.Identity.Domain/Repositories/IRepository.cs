namespace AQPlunch.Identity.Domain.Repositories;

public interface IRepository<T> where T : class
{
    Task<List<T>> ObtenerTodos();
    Task<T?> ObtenerPorId(Guid id);
    Task<bool> Existe(Guid id);
    Task Agregar(T entidad);
    void Actualizar(T entidad);
    void Eliminar(T entidad);
}
