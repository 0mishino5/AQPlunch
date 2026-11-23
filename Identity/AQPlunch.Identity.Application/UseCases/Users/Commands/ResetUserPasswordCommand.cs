using AQPlunch.Identity.Application.Security;
using AQPlunch.Identity.Domain.Entities;
using AQPlunch.Identity.Domain.Exceptions;
using AQPlunch.Identity.Domain.Repositories;
using MediatR;

namespace AQPlunch.Identity.Application.UseCases.Users.Commands;

public class ResetUserPasswordCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public string NewPassword { get; set; } = null!;
}

internal sealed class ResetUserPasswordCommandHandler(IAuthService authService, IUnitOfWork unitOfWork) : IRequestHandler<ResetUserPasswordCommand, Unit>
{
    public async Task<Unit> Handle(ResetUserPasswordCommand request, CancellationToken cancellationToken)
    {
        User usuario = await unitOfWork.Users.ObtenerPorId(request.UserId) ?? throw new RecursoNoEncontradoException($"No existe el usuario con Id {request.UserId}");

        if (string.IsNullOrWhiteSpace(request.NewPassword) || request.NewPassword.Length < 8)
        {
            throw new ReglaNegocioException("La contrasena debe tener al menos 8 caracteres");
        }

        usuario.PasswordHash = authService.GenerarHash(request.NewPassword);

        unitOfWork.Users.Actualizar(usuario);
        await unitOfWork.GuardarCambios();

        return Unit.Value;
    }
}
