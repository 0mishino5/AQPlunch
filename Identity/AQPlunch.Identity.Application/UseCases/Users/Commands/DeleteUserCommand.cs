using AQPlunch.Identity.Domain.Catalogs;
using AQPlunch.Identity.Domain.Entities;
using AQPlunch.Identity.Domain.Exceptions;
using AQPlunch.Identity.Domain.Repositories;
using MediatR;

namespace AQPlunch.Identity.Application.UseCases.Users.Commands;

public class DeleteUserCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
}

internal sealed class DeleteUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserCommand, Unit>
{
    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        User usuario = await unitOfWork.Users.ObtenerPorId(request.UserId) ?? throw new RecursoNoEncontradoException($"No existe el usuario con Id {request.UserId}");

        bool esAdminActivo = usuario.IsActive && usuario.UserRoles.Any(ur => ur.Role.RoleName == NombreRol.Admin);

        if (esAdminActivo && await unitOfWork.Users.ContarActivosConRol(NombreRol.Admin) <= 1)
        {
            throw new ReglaNegocioException("No se puede eliminar al unico Admin activo");
        }

        unitOfWork.Users.Eliminar(usuario);
        await unitOfWork.GuardarCambios();

        return Unit.Value;
    }
}
