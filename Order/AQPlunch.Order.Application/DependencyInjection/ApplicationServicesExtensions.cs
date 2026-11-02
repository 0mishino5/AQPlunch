using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace AQPlunch.OrderService.Application.DependencyInjection;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

        return services;
    }
}
