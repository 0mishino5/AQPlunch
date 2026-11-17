namespace AQPlunch.Sales.Application.Security;

public interface IUsuarioActual
{
    bool EsAdmin();
    Guid? ObtenerRestaurantId();
    bool PuedeAdministrar(Guid restaurantId);
}
