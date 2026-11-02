using AQPlunch.Identity.Domain.Entities;
using AQPlunch.Identity.Domain.Exceptions;
using AQPlunch.Identity.Domain.Repositories;
using MediatR;

namespace AQPlunch.Identity.Application.UseCases.Users.Queries;

public class GetUserByIdQuery : IRequest<User>
{
    public Guid UserId { get; set; }
}

internal sealed class GetUserByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUserByIdQuery, User>
{
    public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        return await unitOfWork.Users.ObtenerPorId(request.UserId) ?? throw new RecursoNoEncontradoException($"No existe el usuario con Id {request.UserId}");
    }
}
