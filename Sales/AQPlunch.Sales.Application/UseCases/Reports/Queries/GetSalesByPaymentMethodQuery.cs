using AQPlunch.Sales.Application.Security;
using AQPlunch.Sales.Domain.Exceptions;
using AQPlunch.Sales.Domain.Reports;
using AQPlunch.Sales.Domain.Repositories;
using MediatR;

namespace AQPlunch.Sales.Application.UseCases.Reports.Queries;

public class GetSalesByPaymentMethodQuery : IRequest<List<PaymentMethodSales>>
{
    public Guid RestaurantId { get; set; }
    public DateTime? Desde { get; set; }
    public DateTime? Hasta { get; set; }
}

internal sealed class GetSalesByPaymentMethodQueryHandler(IUnitOfWork unitOfWork, IUsuarioActual usuarioActual) : IRequestHandler<GetSalesByPaymentMethodQuery, List<PaymentMethodSales>>
{
    public async Task<List<PaymentMethodSales>> Handle(GetSalesByPaymentMethodQuery request, CancellationToken cancellationToken)
    {
        if (!usuarioActual.PuedeAdministrar(request.RestaurantId))
        {
            throw new AccesoDenegadoException("No puedes ver los reportes de otro restaurante");
        }

        if (request.Desde != null && request.Hasta != null && request.Desde > request.Hasta)
        {
            throw new ReglaNegocioException("La fecha desde no puede ser mayor que la fecha hasta");
        }

        return await unitOfWork.Sales.ObtenerVentasPorMetodoDePago(request.RestaurantId, request.Desde, request.Hasta);
    }
}
