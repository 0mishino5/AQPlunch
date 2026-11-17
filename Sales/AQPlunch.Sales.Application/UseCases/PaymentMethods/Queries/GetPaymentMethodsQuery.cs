using AQPlunch.Sales.Domain.Entities;
using AQPlunch.Sales.Domain.Repositories;
using MediatR;

namespace AQPlunch.Sales.Application.UseCases.PaymentMethods.Queries;

public class GetPaymentMethodsQuery : IRequest<List<PaymentMethod>>
{
}

internal sealed class GetPaymentMethodsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetPaymentMethodsQuery, List<PaymentMethod>>
{
    public async Task<List<PaymentMethod>> Handle(GetPaymentMethodsQuery request, CancellationToken cancellationToken)
    {
        return await unitOfWork.PaymentMethods.ObtenerTodos();
    }
}
