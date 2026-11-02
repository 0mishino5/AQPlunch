using AQPlunch.Identity.Domain.Entities;

namespace AQPlunch.Identity.Application.Security;

public interface IAuthService
{
    string GenerarHash(string password);
    bool VerificarPassword(string password, string hashGuardado);
    string GenerarToken(User usuario);
}
