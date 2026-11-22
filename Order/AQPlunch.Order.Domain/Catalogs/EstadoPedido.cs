namespace AQPlunch.OrderService.Domain.Catalogs;

public static class EstadoPedido
{
    public const string Pendiente = "pendiente";
    public const string EnPreparacion = "en_preparacion";
    public const string Listo = "listo";
    public const string Entregado = "entregado";
    public const string Cancelado = "cancelado";

    private static readonly Dictionary<string, string[]> Siguientes = new Dictionary<string, string[]>
    {
        { Pendiente, new[] { EnPreparacion, Cancelado } },
        { EnPreparacion, new[] { Listo, Cancelado } },
        { Listo, new[] { Entregado } },
        { Entregado, Array.Empty<string>() },
        { Cancelado, Array.Empty<string>() }
    };

    public static bool Existe(string estado)
    {
        return Siguientes.ContainsKey(estado);
    }

    public static bool PuedePasar(string actual, string nuevo)
    {
        return Siguientes.TryGetValue(actual, out string[]? siguientes) && siguientes.Contains(nuevo);
    }
}
