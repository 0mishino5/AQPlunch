using AQPlunch.RestaurantService.Infrastructure.Database;

namespace AQPlunch.RestaurantService.Infrastructure.Repositories;

public class GenericRepository<T> : Repository<T> where T : class
{
    public GenericRepository(RestaurantDbContext context) : base(context)
    {
    }
}
