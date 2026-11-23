using AQPlunch.RestaurantService.Application.Security;
using AQPlunch.RestaurantService.Domain.Entities;
using AQPlunch.RestaurantService.Domain.Exceptions;
using AQPlunch.RestaurantService.Domain.Repositories;
using MediatR;

namespace AQPlunch.RestaurantService.Application.UseCases.Products.Commands;

public class ChangeProductAvailabilityCommand : IRequest<Unit>
{
    public Guid ProductId { get; set; }
    public bool? IsAvailable { get; set; }
}

internal sealed class ChangeProductAvailabilityCommandHandler(IUnitOfWork unitOfWork, IUsuarioActual usuarioActual) : IRequestHandler<ChangeProductAvailabilityCommand, Unit>
{
    public async Task<Unit> Handle(ChangeProductAvailabilityCommand request, CancellationToken cancellationToken)
    {
        Product producto = await unitOfWork.Products.ObtenerPorId(request.ProductId) ?? throw new RecursoNoEncontradoException($"No existe el producto con Id {request.ProductId}");

        if (!usuarioActual.PuedeAdministrar(producto.Category.RestaurantId))
        {
            throw new AccesoDenegadoException("No puedes modificar los productos de otro restaurante");
        }

        if (request.IsAvailable == null)
        {
            throw new ReglaNegocioException("Indica si el producto esta disponible");
        }

        producto.IsAvailable = request.IsAvailable.Value;

        unitOfWork.Products.Actualizar(producto);
        await unitOfWork.GuardarCambios();

        return Unit.Value;
    }
}
