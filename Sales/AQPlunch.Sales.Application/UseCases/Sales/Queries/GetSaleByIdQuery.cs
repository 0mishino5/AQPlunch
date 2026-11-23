using AQPlunch.Sales.Application.Security;
using AQPlunch.Sales.Domain.Entities;
using AQPlunch.Sales.Domain.Exceptions;
using AQPlunch.Sales.Domain.Repositories;
using MediatR;

namespace AQPlunch.Sales.Application.UseCases.Sales.Queries;

public class GetSaleByIdQuery : IRequest<Sale>
{
    public Guid SaleId { get; set; }
}

internal sealed class GetSaleByIdQueryHandler(IUnitOfWork unitOfWork, IUsuarioActual usuarioActual) : IRequestHandler<GetSaleByIdQuery, Sale>
{
    public async Task<Sale> Handle(GetSaleByIdQuery request, CancellationToken cancellationToken)
    {
        Sale venta = await unitOfWork.Sales.ObtenerPorId(request.SaleId) ?? throw new RecursoNoEncontradoException($"No existe la venta con Id {request.SaleId}");

        if (!usuarioActual.PuedeAdministrar(venta.RestaurantId))
        {
            throw new AccesoDenegadoException("No puedes ver las ventas de otro restaurante");
        }

        return venta;
    }
}
