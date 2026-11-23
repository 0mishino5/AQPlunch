using AQPlunch.OrderService.Application.Clients;
using AQPlunch.OrderService.Domain.Catalogs;
using AQPlunch.OrderService.Domain.Entities;
using AQPlunch.OrderService.Domain.Exceptions;
using AQPlunch.OrderService.Domain.Repositories;
using MediatR;

namespace AQPlunch.OrderService.Application.UseCases.Orders.Commands;

public class AddOrderCommand : IRequest<Guid>
{
    public Guid QrToken { get; set; }
    public string? Note { get; set; }
    public List<AddOrderItem> Items { get; set; } = new List<AddOrderItem>();
}

public class AddOrderItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}

internal sealed class AddOrderCommandHandler(IUnitOfWork unitOfWork, IRestaurantClient restaurantClient) : IRequestHandler<AddOrderCommand, Guid>
{
    public async Task<Guid> Handle(AddOrderCommand request, CancellationToken cancellationToken)
    {
        if (request.Items.Count == 0)
        {
            throw new ReglaNegocioException("El pedido debe tener al menos un producto");
        }

        if (request.Items.Any(i => i.Quantity < 1 || i.Quantity > 50))
        {
            throw new ReglaNegocioException("La cantidad de cada producto debe estar entre 1 y 50");
        }

        if (request.Note != null && request.Note.Length > 200)
        {
            throw new ReglaNegocioException("La nota tiene como maximo 200 caracteres");
        }

        List<AddOrderItem> items = request.Items
            .GroupBy(i => i.ProductId)
            .Select(g => new AddOrderItem { ProductId = g.Key, Quantity = g.Sum(i => i.Quantity) })
            .ToList();

        TableInfo mesa = await restaurantClient.ObtenerMesaPorToken(request.QrToken);
        List<ProductInfo> productos = await restaurantClient.ObtenerProductosPorIds(items.Select(i => i.ProductId).ToList());

        Order pedido = new Order
        {
            OrderId = Guid.NewGuid(),
            RestaurantId = mesa.RestaurantId,
            TableId = mesa.TableId,
            TableNumber = mesa.TableNumber,
            Status = EstadoPedido.Pendiente,
            Note = string.IsNullOrWhiteSpace(request.Note) ? null : request.Note.Trim()
        };

        foreach (AddOrderItem item in items)
        {
            ProductInfo producto = productos.FirstOrDefault(p => p.ProductId == item.ProductId) ?? throw new RecursoNoEncontradoException($"No existe el producto con Id {item.ProductId}");

            if (producto.RestaurantId != mesa.RestaurantId)
            {
                throw new ReglaNegocioException($"El producto {producto.Name} no es de {mesa.RestaurantName}");
            }

            if (!producto.IsAvailable)
            {
                throw new ReglaNegocioException($"El producto {producto.Name} no esta disponible por ahora");
            }

            pedido.OrderDetails.Add(new OrderDetail
            {
                OrderDetailId = Guid.NewGuid(),
                ProductId = producto.ProductId,
                ProductName = producto.Name,
                UnitPrice = producto.Price,
                Quantity = item.Quantity,
                Subtotal = producto.Price * item.Quantity
            });
        }

        pedido.Total = pedido.OrderDetails.Sum(d => d.Subtotal);

        await unitOfWork.Orders.Agregar(pedido);
        await unitOfWork.GuardarCambios();

        return pedido.OrderId;
    }
}
