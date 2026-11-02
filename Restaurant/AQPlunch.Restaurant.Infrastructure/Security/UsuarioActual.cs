using System.Security.Claims;
using AQPlunch.RestaurantService.Application.Security;
using AQPlunch.RestaurantService.Domain.Catalogs;
using Microsoft.AspNetCore.Http;

namespace AQPlunch.RestaurantService.Infrastructure.Security;

public class UsuarioActual : IUsuarioActual
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UsuarioActual(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool EsAdmin()
    {
        ClaimsPrincipal? usuario = _httpContextAccessor.HttpContext?.User;

        return usuario != null && usuario.IsInRole(NombreRol.Admin);
    }

    public Guid? ObtenerRestaurantId()
    {
        string? valor = _httpContextAccessor.HttpContext?.User.FindFirstValue(NombreClaim.RestaurantId);

        if (valor == null)
        {
            return null;
        }

        return Guid.Parse(valor);
    }

    public bool PuedeAdministrar(Guid restaurantId)
    {
        return EsAdmin() || ObtenerRestaurantId() == restaurantId;
    }
}
