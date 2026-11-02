using AQPlunch.Identity.Domain.Entities;
using AQPlunch.Identity.Domain.Exceptions;
using AQPlunch.Identity.Domain.Repositories;
using MediatR;

namespace AQPlunch.Identity.Application.UseCases.Roles.Queries;

public class GetRoleByIdQuery : IRequest<Role>
{
    public Guid RoleId { get; set; }
}

internal sealed class GetRoleByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetRoleByIdQuery, Role>
{
    public async Task<Role> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        return await unitOfWork.Roles.ObtenerPorId(request.RoleId) ?? throw new RecursoNoEncontradoException($"No existe el rol con Id {request.RoleId}");
    }
}
