using AQPlunch.Identity.Application.Security;
using AQPlunch.Identity.Domain.Entities;
using AQPlunch.Identity.Domain.Exceptions;
using AQPlunch.Identity.Domain.Repositories;
using MediatR;

namespace AQPlunch.Identity.Application.UseCases.Auth.Commands;

public class ChangePasswordCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public string CurrentPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}

internal sealed class ChangePasswordCommandHandler(IAuthService authService, IUnitOfWork unitOfWork) : IRequestHandler<ChangePasswordCommand, Unit>
{
    public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        User usuario = await unitOfWork.Users.ObtenerPorId(request.UserId) ?? throw new RecursoNoEncontradoException($"No existe el usuario con Id {request.UserId}");

        if (!usuario.IsActive)
        {
            throw new CredencialesInvalidasException("La cuenta esta desactivada");
        }

        if (string.IsNullOrWhiteSpace(request.CurrentPassword) || !authService.VerificarPassword(request.CurrentPassword, usuario.PasswordHash))
        {
            throw new ReglaNegocioException("La contrasena actual no es correcta");
        }

        if (string.IsNullOrWhiteSpace(request.NewPassword) || request.NewPassword.Length < 8)
        {
            throw new ReglaNegocioException("La contrasena debe tener al menos 8 caracteres");
        }

        if (request.NewPassword == request.CurrentPassword)
        {
            throw new ReglaNegocioException("La nueva contrasena debe ser distinta de la actual");
        }

        usuario.PasswordHash = authService.GenerarHash(request.NewPassword);

        unitOfWork.Users.Actualizar(usuario);
        await unitOfWork.GuardarCambios();

        return Unit.Value;
    }
}
