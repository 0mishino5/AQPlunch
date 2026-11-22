using AQPlunch.OrderService.Application.Security;
using AQPlunch.OrderService.Domain.Catalogs;
using AQPlunch.OrderService.Domain.Entities;
using AQPlunch.OrderService.Domain.Exceptions;
using AQPlunch.OrderService.Domain.Repositories;
using MediatR;

namespace AQPlunch.OrderService.Application.UseCases.Orders.Commands;

public class UpdateOrderStatusCommand : IRequest<Unit>
{
    public Guid OrderId { get; set; }
    public string Status { get; set; } = null!;
}

internal sealed class UpdateOrderStatusCommandHandler(IUnitOfWork unitOfWork, IUsuarioActual usuarioActual) : IRequestHandler<UpdateOrderStatusCommand, Unit>
{
    public async Task<Unit> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        string nuevoEstado = (request.Status ?? string.Empty).Trim().ToLowerInvariant();

        if (!EstadoPedido.Existe(nuevoEstado))
        {
            throw new ReglaNegocioException("El estado debe ser pendiente, en_preparacion, listo, entregado o cancelado");
        }

        Order pedido = await unitOfWork.Orders.ObtenerPorId(request.OrderId) ?? throw new RecursoNoEncontradoException($"No existe el pedido con Id {request.OrderId}");

        if (!usuarioActual.PuedeAdministrar(pedido.RestaurantId))
        {
            throw new AccesoDenegadoException("No puedes modificar los pedidos de otro restaurante");
        }

        if (!EstadoPedido.PuedePasar(pedido.Status, nuevoEstado))
        {
            throw new ReglaNegocioException($"Un pedido {pedido.Status} no puede pasar a {nuevoEstado}");
        }

        pedido.Status = nuevoEstado;
        pedido.UpdatedAt = DateTime.Now;

        unitOfWork.Orders.Actualizar(pedido);
        await unitOfWork.GuardarCambios();

        return Unit.Value;
    }
}
