using AQPlunch.RestaurantService.Application.Security;
using AQPlunch.RestaurantService.Domain.Entities;
using AQPlunch.RestaurantService.Domain.Exceptions;
using AQPlunch.RestaurantService.Domain.Repositories;
using MediatR;

namespace AQPlunch.RestaurantService.Application.UseCases.Restaurants.Queries;

public class GetRestaurantByIdQuery : IRequest<Restaurant>
{
    public Guid RestaurantId { get; set; }
}

internal sealed class GetRestaurantByIdQueryHandler(IUnitOfWork unitOfWork, IUsuarioActual usuarioActual) : IRequestHandler<GetRestaurantByIdQuery, Restaurant>
{
    public async Task<Restaurant> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
    {
        if (!usuarioActual.PuedeAdministrar(request.RestaurantId))
        {
            throw new AccesoDenegadoException("No puedes ver los datos de otro restaurante");
        }

        return await unitOfWork.Repositorio<Restaurant>().ObtenerPorId(request.RestaurantId) ?? throw new RecursoNoEncontradoException($"No existe el restaurante con Id {request.RestaurantId}");
    }
}
