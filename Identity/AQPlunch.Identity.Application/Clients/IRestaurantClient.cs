namespace AQPlunch.Identity.Application.Clients;

public interface IRestaurantClient
{
    Task<RestaurantInfo> ObtenerRestaurante(Guid restaurantId);
}
