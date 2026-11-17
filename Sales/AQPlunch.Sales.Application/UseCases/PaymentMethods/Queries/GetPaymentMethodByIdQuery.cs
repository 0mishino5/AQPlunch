using AQPlunch.Sales.Domain.Entities;
using AQPlunch.Sales.Domain.Exceptions;
using AQPlunch.Sales.Domain.Repositories;
using MediatR;

namespace AQPlunch.Sales.Application.UseCases.PaymentMethods.Queries;

public class GetPaymentMethodByIdQuery : IRequest<PaymentMethod>
{
    public Guid PaymentMethodId { get; set; }
}

internal sealed class GetPaymentMethodByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetPaymentMethodByIdQuery, PaymentMethod>
{
    public async Task<PaymentMethod> Handle(GetPaymentMethodByIdQuery request, CancellationToken cancellationToken)
    {
        return await unitOfWork.PaymentMethods.ObtenerPorId(request.PaymentMethodId) ?? throw new RecursoNoEncontradoException($"No existe el metodo de pago con Id {request.PaymentMethodId}");
    }
}
