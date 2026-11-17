using AQPlunch.OrderService.Infrastructure.Database;

namespace AQPlunch.OrderService.Infrastructure.Repositories;

public class GenericRepository<T> : Repository<T> where T : class
{
    public GenericRepository(OrderDbContext context) : base(context)
    {
    }
}
