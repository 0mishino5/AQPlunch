namespace AQPlunch.OrderService.Domain.Exceptions;

public class RecursoNoEncontradoException : Exception
{
    public RecursoNoEncontradoException(string mensaje) : base(mensaje)
    {
    }
}
