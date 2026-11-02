using AQPlunch.RestaurantService.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AQPlunch.RestaurantService.Api.ExceptionHandling;

public class ManejadorDeExcepciones : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        IActionResult? resultado = context.Exception switch
        {
            RecursoNoEncontradoException => new NotFoundObjectResult(new { message = context.Exception.Message }),
            ReglaNegocioException => new BadRequestObjectResult(new { message = context.Exception.Message }),
            AccesoDenegadoException => new ObjectResult(new { message = context.Exception.Message }) { StatusCode = StatusCodes.Status403Forbidden },
            _ => null
        };

        if (resultado == null)
        {
            return;
        }

        context.Result = resultado;
        context.ExceptionHandled = true;
    }
}
