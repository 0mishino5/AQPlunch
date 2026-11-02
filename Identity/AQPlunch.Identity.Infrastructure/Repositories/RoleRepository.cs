using AQPlunch.Identity.Domain.Entities;
using AQPlunch.Identity.Domain.Repositories;
using AQPlunch.Identity.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AQPlunch.Identity.Infrastructure.Repositories;

public class RoleRepository : Repository<Role>, IRoleRepository
{
    public RoleRepository(IdentityDbContext context) : base(context)
    {
    }

    public override async Task<List<Role>> ObtenerTodos()
    {
        return await Consulta()
            .OrderBy(r => r.RoleName)
            .ToListAsync();
    }

    public async Task<Role?> ObtenerPorNombre(string nombre)
    {
        return await Consulta()
            .FirstOrDefaultAsync(r => r.RoleName.ToLower() == nombre.ToLower());
    }

    protected override IQueryable<Role> Consulta()
    {
        return _dbSet.Include(r => r.UserRoles);
    }
}
