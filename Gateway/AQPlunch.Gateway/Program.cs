var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new
{
    servicio = "AQPlunch Gateway",
    servicios = new[]
    {
        "IdentityService (5001): api/auth, api/usuarios, api/roles",
        "RestaurantService (5002): api/restaurantes, api/mesas, api/categorias, api/productos",
        "OrderService (5003): api/pedidos",
        "SalesService (5004): api/ventas, api/reportes"
    }
}));

app.MapReverseProxy();

app.Run();
