using AQPlunch.RestaurantService.Application.Security;
using AQPlunch.RestaurantService.Domain.Entities;
using AQPlunch.RestaurantService.Domain.Exceptions;
using AQPlunch.RestaurantService.Domain.Repositories;
using MediatR;

namespace AQPlunch.RestaurantService.Application.UseCases.Categories.Queries;

public class GetCategoriesByRestaurantQuery : IRequest<List<Category>>
{
    public Guid RestaurantId { get; set; }
}

internal sealed class GetCategoriesByRestaurantQueryHandler(IUnitOfWork unitOfWork, IUsuarioActual usuarioActual) : IRequestHandler<GetCategoriesByRestaurantQuery, List<Category>>
{
    public async Task<List<Category>> Handle(GetCategoriesByRestaurantQuery request, CancellationToken cancellationToken)
    {
        if (!usuarioActual.PuedeAdministrar(request.RestaurantId))
        {
            throw new AccesoDenegadoException("No puedes ver las categorias de otro restaurante");
        }

        return await unitOfWork.Categories.ObtenerPorRestaurante(request.RestaurantId);
    }
}
