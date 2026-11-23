namespace AQPlunch.Sales.Application.Clients;

public interface IOrderClient
{
    Task<OrderInfo> ObtenerPedido(Guid orderId);
}
