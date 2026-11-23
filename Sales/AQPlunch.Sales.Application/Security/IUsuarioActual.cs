namespace AQPlunch.Sales.Application.Security;

public interface IUsuarioActual
{
    Guid ObtenerUserId();
    bool EsAdmin();
    Guid? ObtenerRestaurantId();
    bool PuedeAdministrar(Guid restaurantId);
}
