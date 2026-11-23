using AQPlunch.Sales.Application.Clients;
using AQPlunch.Sales.Application.Security;
using AQPlunch.Sales.Domain.Catalogs;
using AQPlunch.Sales.Domain.Entities;
using AQPlunch.Sales.Domain.Exceptions;
using AQPlunch.Sales.Domain.Repositories;
using MediatR;

namespace AQPlunch.Sales.Application.UseCases.Sales.Commands;

public class AddSaleCommand : IRequest<Guid>
{
    public Guid OrderId { get; set; }
    public Guid PaymentMethodId { get; set; }
}

internal sealed class AddSaleCommandHandler(IUnitOfWork unitOfWork, IUsuarioActual usuarioActual, IOrderClient orderClient) : IRequestHandler<AddSaleCommand, Guid>
{
    public async Task<Guid> Handle(AddSaleCommand request, CancellationToken cancellationToken)
    {
        PaymentMethod metodo = await unitOfWork.PaymentMethods.ObtenerPorId(request.PaymentMethodId) ?? throw new RecursoNoEncontradoException($"No existe el metodo de pago con Id {request.PaymentMethodId}");

        if (!metodo.IsActive)
        {
            throw new ReglaNegocioException($"El metodo de pago {metodo.Name} no esta activo");
        }

        OrderInfo pedido = await orderClient.ObtenerPedido(request.OrderId);

        if (!usuarioActual.PuedeAdministrar(pedido.RestaurantId))
        {
            throw new AccesoDenegadoException("No puedes registrar ventas de otro restaurante");
        }

        if (pedido.Status != EstadoPedido.Entregado)
        {
            throw new ReglaNegocioException("Solo se puede registrar la venta de un pedido entregado");
        }

        if (await unitOfWork.Sales.ExistePorPedido(pedido.OrderId))
        {
            throw new ReglaNegocioException("Ese pedido ya tiene una venta registrada");
        }

        Sale venta = new Sale
        {
            SaleId = Guid.NewGuid(),
            OrderId = pedido.OrderId,
            RestaurantId = pedido.RestaurantId,
            PaymentMethodId = metodo.PaymentMethodId,
            RegisteredBy = usuarioActual.ObtenerUserId(),
            Total = pedido.Total,
            SaleDetails = pedido.OrderDetails.Select(d => new SaleDetail
            {
                SaleDetailId = Guid.NewGuid(),
                ProductId = d.ProductId,
                ProductName = d.ProductName,
                UnitPrice = d.UnitPrice,
                Quantity = d.Quantity,
                Subtotal = d.Subtotal
            }).ToList()
        };

        await unitOfWork.Sales.Agregar(venta);
        await unitOfWork.GuardarCambios();

        return venta.SaleId;
    }
}
