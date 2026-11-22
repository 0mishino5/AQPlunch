namespace AQPlunch.OrderService.Domain.Exceptions;

public class AccesoDenegadoException : Exception
{
    public AccesoDenegadoException(string mensaje) : base(mensaje)
    {
    }
}
