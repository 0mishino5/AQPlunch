using AQPlunch.Identity.Domain.Entities;
using AQPlunch.Identity.Domain.Repositories;
using MediatR;

namespace AQPlunch.Identity.Application.UseCases.Users.Queries;

public class GetUsersQuery : IRequest<List<User>>
{
}

internal sealed class GetUsersQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUsersQuery, List<User>>
{
    public async Task<List<User>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return await unitOfWork.Users.ObtenerTodos();
    }
}
