using AQPlunch.RestaurantService.Domain.Entities;
using AQPlunch.RestaurantService.Domain.Repositories;
using MediatR;

namespace AQPlunch.RestaurantService.Application.UseCases.Restaurants.Queries;

public class GetRestaurantsQuery : IRequest<List<Restaurant>>
{
}

internal sealed class GetRestaurantsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetRestaurantsQuery, List<Restaurant>>
{
    public async Task<List<Restaurant>> Handle(GetRestaurantsQuery request, CancellationToken cancellationToken)
    {
        List<Restaurant> restaurantes = await unitOfWork.Repositorio<Restaurant>().ObtenerTodos();

        return restaurantes.OrderBy(r => r.Name).ToList();
    }
}
