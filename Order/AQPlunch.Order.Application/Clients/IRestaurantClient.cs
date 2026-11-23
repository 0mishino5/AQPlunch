namespace AQPlunch.OrderService.Application.Clients;

public interface IRestaurantClient
{
    Task<TableInfo> ObtenerMesaPorToken(Guid qrToken);
    Task<List<ProductInfo>> ObtenerProductosPorIds(List<Guid> ids);
}
