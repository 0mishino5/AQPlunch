using AQPlunch.OrderService.Application.Clients;
using AQPlunch.OrderService.Application.Security;
using AQPlunch.OrderService.Domain.Repositories;
using AQPlunch.OrderService.Infrastructure.Clients;
using AQPlunch.OrderService.Infrastructure.Database;
using AQPlunch.OrderService.Infrastructure.Repositories;
using AQPlunch.OrderService.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AQPlunch.OrderService.Infrastructure.DependencyInjection;

public static class InfrastructureServicesExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrderDbContext>(options =>
        {
            string? connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddHttpContextAccessor();
        services.AddScoped<IUsuarioActual, UsuarioActual>();

        services.AddHttpClient<IRestaurantClient, RestaurantClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["Servicios:Restaurant"]!);
            client.Timeout = TimeSpan.FromSeconds(10);
        });

        return services;
    }
}
