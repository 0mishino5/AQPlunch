using AQPlunch.RestaurantService.Application.Security;
using AQPlunch.RestaurantService.Domain.Entities;
using AQPlunch.RestaurantService.Domain.Exceptions;
using AQPlunch.RestaurantService.Domain.Repositories;
using MediatR;

namespace AQPlunch.RestaurantService.Application.UseCases.Products.Queries;

public class SearchProductsQuery : IRequest<List<Product>>
{
    public Guid RestaurantId { get; set; }
    public string? Texto { get; set; }
    public Guid? CategoriaId { get; set; }
    public bool SoloDisponibles { get; set; }
}

internal sealed class SearchProductsQueryHandler(IUnitOfWork unitOfWork, IUsuarioActual usuarioActual) : IRequestHandler<SearchProductsQuery, List<Product>>
{
    public async Task<List<Product>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
    {
        if (!usuarioActual.PuedeAdministrar(request.RestaurantId))
        {
            throw new AccesoDenegadoException("No puedes ver los productos de otro restaurante");
        }

        return await unitOfWork.Products.Buscar(request.RestaurantId, request.Texto, request.CategoriaId, request.SoloDisponibles);
    }
}
