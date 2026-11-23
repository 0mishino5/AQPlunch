using AQPlunch.Sales.Infrastructure.Database;

namespace AQPlunch.Sales.Infrastructure.Repositories;

public class GenericRepository<T> : Repository<T> where T : class
{
    public GenericRepository(SalesDbContext context) : base(context)
    {
    }
}
