using AQPlunch.Identity.Application.Security;
using AQPlunch.Identity.Domain.Entities;
using AQPlunch.Identity.Domain.Exceptions;
using AQPlunch.Identity.Domain.Repositories;
using MediatR;

namespace AQPlunch.Identity.Application.UseCases.Auth.Queries;

public class LoginQuery : IRequest<string>
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}

internal sealed class LoginQueryHandler(IAuthService authService, IUnitOfWork unitOfWork) : IRequestHandler<LoginQuery, string>
{
    public async Task<string> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        User usuario = await unitOfWork.Users.ObtenerPorUsername(request.Username) ?? throw new CredencialesInvalidasException("Usuario o contrasena incorrectos");

        if (!authService.VerificarPassword(request.Password, usuario.PasswordHash))
        {
            throw new CredencialesInvalidasException("Usuario o contrasena incorrectos");
        }

        if (!usuario.IsActive)
        {
            throw new CredencialesInvalidasException("La cuenta esta desactivada");
        }

        return authService.GenerarToken(usuario);
    }
}
