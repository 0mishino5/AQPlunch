using AQPlunch.Sales.Domain.Entities;
using AQPlunch.Sales.Domain.Exceptions;
using AQPlunch.Sales.Domain.Repositories;
using MediatR;

namespace AQPlunch.Sales.Application.UseCases.PaymentMethods.Commands;

public class DeletePaymentMethodCommand : IRequest<Unit>
{
    public Guid PaymentMethodId { get; set; }
}

internal sealed class DeletePaymentMethodCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeletePaymentMethodCommand, Unit>
{
    public async Task<Unit> Handle(DeletePaymentMethodCommand request, CancellationToken cancellationToken)
    {
        PaymentMethod metodo = await unitOfWork.PaymentMethods.ObtenerPorId(request.PaymentMethodId) ?? throw new RecursoNoEncontradoException($"No existe el metodo de pago con Id {request.PaymentMethodId}");

        if (await unitOfWork.Sales.ExistenConMetodoDePago(request.PaymentMethodId))
        {
            throw new ReglaNegocioException("Este metodo de pago ya se uso en ventas. Desactivalo en lugar de eliminarlo");
        }

        unitOfWork.PaymentMethods.Eliminar(metodo);
        await unitOfWork.GuardarCambios();

        return Unit.Value;
    }
}
