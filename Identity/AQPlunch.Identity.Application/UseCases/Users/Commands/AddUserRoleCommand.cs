using AQPlunch.Identity.Domain.Catalogs;
using AQPlunch.Identity.Domain.Entities;
using AQPlunch.Identity.Domain.Exceptions;
using AQPlunch.Identity.Domain.Repositories;
using MediatR;

namespace AQPlunch.Identity.Application.UseCases.Users.Commands;

public class AddUserRoleCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}

internal sealed class AddUserRoleCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddUserRoleCommand, Unit>
{
    public async Task<Unit> Handle(AddUserRoleCommand request, CancellationToken cancellationToken)
    {
        User usuario = await unitOfWork.Users.ObtenerPorId(request.UserId) ?? throw new RecursoNoEncontradoException($"No existe el usuario con Id {request.UserId}");
        Role rol = await unitOfWork.Roles.ObtenerPorId(request.RoleId) ?? throw new RecursoNoEncontradoException($"No existe el rol con Id {request.RoleId}");

        if (await unitOfWork.Users.ObtenerAsignacion(request.UserId, request.RoleId) != null)
        {
            throw new ReglaNegocioException($"El usuario ya tiene el rol {rol.RoleName}");
        }

        if (rol.RoleName == NombreRol.Restaurante && usuario.RestaurantId == null)
        {
            throw new ReglaNegocioException("Para darle el rol Restaurante, el usuario necesita un RestaurantId");
        }

        await unitOfWork.Repositorio<UserRole>().Agregar(new UserRole
        {
            UserId = request.UserId,
            RoleId = request.RoleId,
            AssignedAt = DateTime.Now
        });
        await unitOfWork.GuardarCambios();

        return Unit.Value;
    }
}
