using AQPlunch.Identity.Domain.Entities;
using AQPlunch.Identity.Domain.Repositories;
using MediatR;

namespace AQPlunch.Identity.Application.UseCases.Roles.Queries;

public class GetRolesQuery : IRequest<List<Role>>
{
}

internal sealed class GetRolesQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetRolesQuery, List<Role>>
{
    public async Task<List<Role>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        return await unitOfWork.Roles.ObtenerTodos();
    }
}
