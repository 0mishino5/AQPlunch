using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using AQPlunch.OrderService.Application.Clients;
using AQPlunch.OrderService.Domain.Exceptions;

namespace AQPlunch.OrderService.Infrastructure.Clients;

public class RestaurantClient : IRestaurantClient
{
    private readonly HttpClient _httpClient;

    public RestaurantClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<TableInfo> ObtenerMesaPorToken(Guid qrToken)
    {
        HttpResponseMessage respuesta = await Enviar($"api/mesas/token/{qrToken}");

        return (await respuesta.Content.ReadFromJsonAsync<TableInfo>())!;
    }

    public async Task<List<ProductInfo>> ObtenerProductosPorIds(List<Guid> ids)
    {
        string parametros = string.Join("&", ids.Distinct().Select(id => $"ids={id}"));
        HttpResponseMessage respuesta = await Enviar($"api/productos/por-ids?{parametros}");

        return (await respuesta.Content.ReadFromJsonAsync<List<ProductInfo>>())!;
    }

    private async Task<HttpResponseMessage> Enviar(string ruta)
    {
        HttpResponseMessage respuesta;

        try
        {
            respuesta = await _httpClient.GetAsync(ruta);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            throw new ServicioNoDisponibleException("RestaurantService no responde, intenta de nuevo en un momento");
        }

        if (respuesta.IsSuccessStatusCode)
        {
            return respuesta;
        }

        string mensaje = await LeerMensaje(respuesta);

        Exception error = respuesta.StatusCode switch
        {
            HttpStatusCode.NotFound => new RecursoNoEncontradoException(mensaje),
            HttpStatusCode.BadRequest => new ReglaNegocioException(mensaje),
            _ => new ServicioNoDisponibleException($"RestaurantService respondio con error {(int)respuesta.StatusCode}")
        };

        throw error;
    }

    private static async Task<string> LeerMensaje(HttpResponseMessage respuesta)
    {
        try
        {
            RespuestaDeError? error = await respuesta.Content.ReadFromJsonAsync<RespuestaDeError>();

            return error?.Message ?? "RestaurantService rechazo la solicitud";
        }
        catch (JsonException)
        {
            return "RestaurantService rechazo la solicitud";
        }
    }

    private sealed class RespuestaDeError
    {
        public string? Message { get; set; }
    }
}
