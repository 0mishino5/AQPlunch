namespace AQPlunch.Sales.Domain.Exceptions;

public class ServicioNoDisponibleException : Exception
{
    public ServicioNoDisponibleException(string mensaje) : base(mensaje)
    {
    }
}
