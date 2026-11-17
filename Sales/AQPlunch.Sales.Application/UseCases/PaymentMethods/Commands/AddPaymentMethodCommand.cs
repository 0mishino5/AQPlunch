using AQPlunch.Sales.Domain.Entities;
using AQPlunch.Sales.Domain.Exceptions;
using AQPlunch.Sales.Domain.Repositories;
using MediatR;

namespace AQPlunch.Sales.Application.UseCases.PaymentMethods.Commands;

public class AddPaymentMethodCommand : IRequest<Guid>
{
    public string Name { get; set; } = null!;
}

internal sealed class AddPaymentMethodCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddPaymentMethodCommand, Guid>
{
    public async Task<Guid> Handle(AddPaymentMethodCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length > 50)
        {
            throw new ReglaNegocioException("El nombre es obligatorio y tiene como maximo 50 caracteres");
        }

        if (await unitOfWork.PaymentMethods.ObtenerPorNombre(request.Name) != null)
        {
            throw new ReglaNegocioException($"Ya existe el metodo de pago {request.Name}");
        }

        PaymentMethod metodo = new PaymentMethod
        {
            PaymentMethodId = Guid.NewGuid(),
            Name = request.Name,
            IsActive = true
        };

        await unitOfWork.PaymentMethods.Agregar(metodo);
        await unitOfWork.GuardarCambios();

        return metodo.PaymentMethodId;
    }
}
