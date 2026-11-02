using AQPlunch.Identity.Domain.Entities;
using AQPlunch.Identity.Domain.Repositories;
using AQPlunch.Identity.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AQPlunch.Identity.Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(IdentityDbContext context) : base(context)
    {
    }

    public override async Task<List<User>> ObtenerTodos()
    {
        return await Consulta()
            .OrderBy(u => u.Username)
            .ToListAsync();
    }

    public async Task<User?> ObtenerPorUsername(string username)
    {
        return await Consulta()
            .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
    }

    public async Task<bool> ExisteUsername(string username)
    {
        return await _dbSet.AnyAsync(u => u.Username.ToLower() == username.ToLower());
    }

    public async Task<bool> ExisteEmail(string email)
    {
        return await _dbSet.AnyAsync(u => u.Email != null && u.Email.ToLower() == email.ToLower());
    }

    public async Task<UserRole?> ObtenerAsignacion(Guid userId, Guid roleId)
    {
        return await _context.UserRoles
            .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
    }

    public async Task<int> ContarActivosConRol(string nombreRol)
    {
        return await _dbSet.CountAsync(u => u.IsActive && u.UserRoles.Any(ur => ur.Role.RoleName == nombreRol));
    }

    protected override IQueryable<User> Consulta()
    {
        return _dbSet
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role);
    }
}
