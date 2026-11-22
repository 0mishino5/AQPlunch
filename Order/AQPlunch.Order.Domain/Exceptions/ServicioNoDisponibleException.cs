namespace AQPlunch.OrderService.Domain.Exceptions;

public class ServicioNoDisponibleException : Exception
{
    public ServicioNoDisponibleException(string mensaje) : base(mensaje)
    {
    }
}
