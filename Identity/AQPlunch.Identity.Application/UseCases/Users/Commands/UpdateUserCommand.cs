using AQPlunch.Identity.Domain.Catalogs;
using AQPlunch.Identity.Domain.Entities;
using AQPlunch.Identity.Domain.Exceptions;
using AQPlunch.Identity.Domain.Repositories;
using MediatR;

namespace AQPlunch.Identity.Application.UseCases.Users.Commands;

public class UpdateUserCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public string? Email { get; set; }
    public string FullName { get; set; } = null!;
    public Guid? RestaurantId { get; set; }
    public bool IsActive { get; set; }
}

internal sealed class UpdateUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateUserCommand, Unit>
{
    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        User usuario = await unitOfWork.Users.ObtenerPorId(request.UserId) ?? throw new RecursoNoEncontradoException($"No existe el usuario con Id {request.UserId}");

        if (string.IsNullOrWhiteSpace(request.FullName))
        {
            throw new ReglaNegocioException("El nombre completo es obligatorio");
        }

        if (!string.IsNullOrWhiteSpace(request.Email) && request.Email != usuario.Email && await unitOfWork.Users.ExisteEmail(request.Email))
        {
            throw new ReglaNegocioException($"El email {request.Email} ya esta en uso");
        }

        bool esAdmin = usuario.UserRoles.Any(ur => ur.Role.RoleName == NombreRol.Admin);
        bool esRestaurante = usuario.UserRoles.Any(ur => ur.Role.RoleName == NombreRol.Restaurante);

        if (esRestaurante && request.RestaurantId == null)
        {
            throw new ReglaNegocioException("El personal de un restaurante necesita el RestaurantId");
        }

        if (esAdmin && usuario.IsActive && !request.IsActive && await unitOfWork.Users.ContarActivosConRol(NombreRol.Admin) <= 1)
        {
            throw new ReglaNegocioException("No se puede desactivar al unico Admin activo");
        }

        usuario.Email = request.Email;
        usuario.FullName = request.FullName;
        usuario.RestaurantId = request.RestaurantId;
        usuario.IsActive = request.IsActive;

        unitOfWork.Users.Actualizar(usuario);
        await unitOfWork.GuardarCambios();

        return Unit.Value;
    }
}
