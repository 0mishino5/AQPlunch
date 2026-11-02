using AQPlunch.Identity.Application.Security;
using AQPlunch.Identity.Domain.Catalogs;
using AQPlunch.Identity.Domain.Entities;
using AQPlunch.Identity.Domain.Exceptions;
using AQPlunch.Identity.Domain.Repositories;
using MediatR;

namespace AQPlunch.Identity.Application.UseCases.Users.Commands;

public class AddUserCommand : IRequest<Guid>
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? Email { get; set; }
    public string FullName { get; set; } = null!;
    public Guid? RestaurantId { get; set; }
    public string RoleName { get; set; } = null!;
}

internal sealed class AddUserCommandHandler(IAuthService authService, IUnitOfWork unitOfWork) : IRequestHandler<AddUserCommand, Guid>
{
    public async Task<Guid> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.FullName))
        {
            throw new ReglaNegocioException("El username y el nombre completo son obligatorios");
        }

        if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 8)
        {
            throw new ReglaNegocioException("La contrasena debe tener al menos 8 caracteres");
        }

        if (await unitOfWork.Users.ExisteUsername(request.Username))
        {
            throw new ReglaNegocioException($"El username {request.Username} ya esta en uso");
        }

        if (!string.IsNullOrWhiteSpace(request.Email) && await unitOfWork.Users.ExisteEmail(request.Email))
        {
            throw new ReglaNegocioException($"El email {request.Email} ya esta en uso");
        }

        Role rol = await unitOfWork.Roles.ObtenerPorNombre(request.RoleName) ?? throw new ReglaNegocioException($"No existe el rol {request.RoleName}");

        if (rol.RoleName == NombreRol.Restaurante && request.RestaurantId == null)
        {
            throw new ReglaNegocioException("El personal de un restaurante necesita el RestaurantId");
        }

        if (rol.RoleName == NombreRol.Admin && request.RestaurantId != null)
        {
            throw new ReglaNegocioException("Un Admin no pertenece a un restaurante");
        }

        User usuario = new User
        {
            UserId = Guid.NewGuid(),
            Username = request.Username,
            PasswordHash = authService.GenerarHash(request.Password),
            Email = request.Email,
            FullName = request.FullName,
            RestaurantId = request.RestaurantId,
            IsActive = true,
            CreatedAt = DateTime.Now
        };

        usuario.UserRoles.Add(new UserRole
        {
            UserId = usuario.UserId,
            RoleId = rol.RoleId,
            AssignedAt = DateTime.Now
        });

        await unitOfWork.Users.Agregar(usuario);
        await unitOfWork.GuardarCambios();

        return usuario.UserId;
    }
}
