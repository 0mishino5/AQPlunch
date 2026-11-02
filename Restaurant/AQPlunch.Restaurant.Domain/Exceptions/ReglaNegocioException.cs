namespace AQPlunch.RestaurantService.Domain.Exceptions;

public class ReglaNegocioException : Exception
{
    public ReglaNegocioException(string mensaje) : base(mensaje)
    {
    }
}
