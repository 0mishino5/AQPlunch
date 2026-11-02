using AQPlunch.RestaurantService.Application.Security;
using AQPlunch.RestaurantService.Domain.Entities;
using AQPlunch.RestaurantService.Domain.Exceptions;
using AQPlunch.RestaurantService.Domain.Repositories;
using MediatR;

namespace AQPlunch.RestaurantService.Application.UseCases.Products.Commands;

public class DeleteProductCommand : IRequest<Unit>
{
    public Guid ProductId { get; set; }
}

internal sealed class DeleteProductCommandHandler(IUnitOfWork unitOfWork, IUsuarioActual usuarioActual) : IRequestHandler<DeleteProductCommand, Unit>
{
    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        Product producto = await unitOfWork.Products.ObtenerPorId(request.ProductId) ?? throw new RecursoNoEncontradoException($"No existe el producto con Id {request.ProductId}");

        if (!usuarioActual.PuedeAdministrar(producto.Category.RestaurantId))
        {
            throw new AccesoDenegadoException("No puedes eliminar los productos de otro restaurante");
        }

        unitOfWork.Products.Eliminar(producto);
        await unitOfWork.GuardarCambios();

        return Unit.Value;
    }
}
