using AQPlunch.Sales.Application.Security;
using AQPlunch.Sales.Domain.Exceptions;
using AQPlunch.Sales.Domain.Reports;
using AQPlunch.Sales.Domain.Repositories;
using MediatR;

namespace AQPlunch.Sales.Application.UseCases.Reports.Queries;

public class GetTopProductsQuery : IRequest<List<ProductSales>>
{
    public Guid RestaurantId { get; set; }
    public DateTime? Desde { get; set; }
    public DateTime? Hasta { get; set; }
    public int Top { get; set; } = 5;
}

internal sealed class GetTopProductsQueryHandler(IUnitOfWork unitOfWork, IUsuarioActual usuarioActual) : IRequestHandler<GetTopProductsQuery, List<ProductSales>>
{
    public async Task<List<ProductSales>> Handle(GetTopProductsQuery request, CancellationToken cancellationToken)
    {
        if (!usuarioActual.PuedeAdministrar(request.RestaurantId))
        {
            throw new AccesoDenegadoException("No puedes ver los reportes de otro restaurante");
        }

        if (request.Desde != null && request.Hasta != null && request.Desde > request.Hasta)
        {
            throw new ReglaNegocioException("La fecha desde no puede ser mayor que la fecha hasta");
        }

        if (request.Top < 1 || request.Top > 50)
        {
            throw new ReglaNegocioException("Top debe estar entre 1 y 50");
        }

        return await unitOfWork.SaleDetails.ObtenerProductosMasVendidos(request.RestaurantId, request.Desde, request.Hasta, request.Top);
    }
}
