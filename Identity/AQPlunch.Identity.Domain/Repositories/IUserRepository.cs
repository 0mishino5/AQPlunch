using AQPlunch.Identity.Domain.Entities;

namespace AQPlunch.Identity.Domain.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> ObtenerPorUsername(string username);
    Task<bool> ExisteUsername(string username);
    Task<bool> ExisteEmail(string email);
    Task<UserRole?> ObtenerAsignacion(Guid userId, Guid roleId);
}
