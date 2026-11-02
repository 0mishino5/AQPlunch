using AQPlunch.Identity.Domain.Catalogs;
using AQPlunch.Identity.Domain.Entities;
using AQPlunch.Identity.Domain.Exceptions;
using AQPlunch.Identity.Domain.Repositories;
using MediatR;

namespace AQPlunch.Identity.Application.UseCases.Users.Commands;

public class DeleteUserRoleCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}

internal sealed class DeleteUserRoleCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserRoleCommand, Unit>
{
    public async Task<Unit> Handle(DeleteUserRoleCommand request, CancellationToken cancellationToken)
    {
        UserRole asignacion = await unitOfWork.Users.ObtenerAsignacion(request.UserId, request.RoleId) ?? throw new RecursoNoEncontradoException("El usuario no tiene asignado ese rol");
        Role rol = await unitOfWork.Roles.ObtenerPorId(request.RoleId) ?? throw new RecursoNoEncontradoException($"No existe el rol con Id {request.RoleId}");
        User usuario = await unitOfWork.Users.ObtenerPorId(request.UserId) ?? throw new RecursoNoEncontradoException($"No existe el usuario con Id {request.UserId}");

        if (rol.RoleName == NombreRol.Admin && usuario.IsActive && await unitOfWork.Users.ContarActivosConRol(NombreRol.Admin) <= 1)
        {
            throw new ReglaNegocioException("No se puede quitar el rol Admin al unico Admin activo");
        }

        unitOfWork.Repositorio<UserRole>().Eliminar(asignacion);
        await unitOfWork.GuardarCambios();

        return Unit.Value;
    }
}
