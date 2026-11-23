using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using AQPlunch.Sales.Application.Clients;
using AQPlunch.Sales.Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace AQPlunch.Sales.Infrastructure.Clients;

public class OrderClient : IOrderClient
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public OrderClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<OrderInfo> ObtenerPedido(Guid orderId)
    {
        HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Get, $"api/pedidos/{orderId}");
        solicitud.Headers.TryAddWithoutValidation("Authorization", _httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString());

        HttpResponseMessage respuesta = await Enviar(solicitud);

        return (await respuesta.Content.ReadFromJsonAsync<OrderInfo>())!;
    }

    private async Task<HttpResponseMessage> Enviar(HttpRequestMessage solicitud)
    {
        HttpResponseMessage respuesta;

        try
        {
            respuesta = await _httpClient.SendAsync(solicitud);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            throw new ServicioNoDisponibleException("OrderService no responde, intenta de nuevo en un momento");
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
            HttpStatusCode.Forbidden => new AccesoDenegadoException(mensaje),
            _ => new ServicioNoDisponibleException($"OrderService respondio con error {(int)respuesta.StatusCode}")
        };

        throw error;
    }

    private static async Task<string> LeerMensaje(HttpResponseMessage respuesta)
    {
        try
        {
            RespuestaDeError? error = await respuesta.Content.ReadFromJsonAsync<RespuestaDeError>();

            return error?.Message ?? "OrderService rechazo la solicitud";
        }
        catch (JsonException)
        {
            return "OrderService rechazo la solicitud";
        }
    }

    private sealed class RespuestaDeError
    {
        public string? Message { get; set; }
    }
}
