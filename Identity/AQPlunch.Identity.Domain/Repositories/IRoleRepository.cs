using AQPlunch.Identity.Domain.Entities;

namespace AQPlunch.Identity.Domain.Repositories;

public interface IRoleRepository : IRepository<Role>
{
    Task<Role?> ObtenerPorNombre(string nombre);
}
