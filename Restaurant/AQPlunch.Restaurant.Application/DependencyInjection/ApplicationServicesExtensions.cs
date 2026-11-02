using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace AQPlunch.RestaurantService.Application.DependencyInjection;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

        return services;
    }
}
