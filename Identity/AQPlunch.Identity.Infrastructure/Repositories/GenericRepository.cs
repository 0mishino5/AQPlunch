using AQPlunch.Identity.Infrastructure.Database;

namespace AQPlunch.Identity.Infrastructure.Repositories;

public class GenericRepository<T> : Repository<T> where T : class
{
    public GenericRepository(IdentityDbContext context) : base(context)
    {
    }
}
