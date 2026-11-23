using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using AQPlunch.Identity.Application.Clients;
using AQPlunch.Identity.Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace AQPlunch.Identity.Infrastructure.Clients;

public class RestaurantClient : IRestaurantClient
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RestaurantClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<RestaurantInfo> ObtenerRestaurante(Guid restaurantId)
    {
        HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Get, $"api/restaurantes/{restaurantId}");
        solicitud.Headers.TryAddWithoutValidation("Authorization", _httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString());

        HttpResponseMessage respuesta = await Enviar(solicitud);

        return (await respuesta.Content.ReadFromJsonAsync<RestaurantInfo>())!;
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
