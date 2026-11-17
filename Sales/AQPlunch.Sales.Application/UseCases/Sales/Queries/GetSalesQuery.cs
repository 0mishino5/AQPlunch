using AQPlunch.Sales.Domain.Entities;
using AQPlunch.Sales.Domain.Repositories;
using MediatR;

namespace AQPlunch.Sales.Application.UseCases.Sales.Queries;

public class GetSalesQuery : IRequest<List<Sale>>
{
}

internal sealed class GetSalesQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetSalesQuery, List<Sale>>
{
    public async Task<List<Sale>> Handle(GetSalesQuery request, CancellationToken cancellationToken)
    {
        return await unitOfWork.Sales.ObtenerTodos();
    }
}
