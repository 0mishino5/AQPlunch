using AQPlunch.Sales.Domain.Entities;
using AQPlunch.Sales.Domain.Exceptions;
using AQPlunch.Sales.Domain.Repositories;
using MediatR;

namespace AQPlunch.Sales.Application.UseCases.PaymentMethods.Commands;

public class UpdatePaymentMethodCommand : IRequest<Unit>
{
    public Guid PaymentMethodId { get; set; }
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; }
}

internal sealed class UpdatePaymentMethodCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdatePaymentMethodCommand, Unit>
{
    public async Task<Unit> Handle(UpdatePaymentMethodCommand request, CancellationToken cancellationToken)
    {
        PaymentMethod metodo = await unitOfWork.PaymentMethods.ObtenerPorId(request.PaymentMethodId) ?? throw new RecursoNoEncontradoException($"No existe el metodo de pago con Id {request.PaymentMethodId}");

        if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length > 50)
        {
            throw new ReglaNegocioException("El nombre es obligatorio y tiene como maximo 50 caracteres");
        }

        PaymentMethod? repetido = await unitOfWork.PaymentMethods.ObtenerPorNombre(request.Name);

        if (repetido != null && repetido.PaymentMethodId != request.PaymentMethodId)
        {
            throw new ReglaNegocioException($"Ya existe el metodo de pago {request.Name}");
        }

        metodo.Name = request.Name;
        metodo.IsActive = request.IsActive;

        unitOfWork.PaymentMethods.Actualizar(metodo);
        await unitOfWork.GuardarCambios();

        return Unit.Value;
    }
}
