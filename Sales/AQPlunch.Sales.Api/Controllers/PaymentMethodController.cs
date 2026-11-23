using AQPlunch.Sales.Application.UseCases.PaymentMethods.Commands;
using AQPlunch.Sales.Application.UseCases.PaymentMethods.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AQPlunch.Sales.Api.Controllers;

[ApiController]
[Authorize(Policy = "Restaurante")]
[Route("api/ventas/metodos-pago")]
public class PaymentMethodController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentMethodController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _mediator.Send(new GetPaymentMethodsQuery()));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await _mediator.Send(new GetPaymentMethodByIdQuery { PaymentMethodId = id }));
    }

    [Authorize(Policy = "Administrador")]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddPaymentMethodCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [Authorize(Policy = "Administrador")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePaymentMethodCommand command)
    {
        command.PaymentMethodId = id;
        await _mediator.Send(command);

        return Ok(new { message = "Metodo de pago actualizado" });
    }

    [Authorize(Policy = "Administrador")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeletePaymentMethodCommand { PaymentMethodId = id });

        return Ok(new { message = "Metodo de pago eliminado" });
    }
}
