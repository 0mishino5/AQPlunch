using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AQPlunch.Identity.Application.Security;
using AQPlunch.Identity.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AQPlunch.Identity.Infrastructure.Security;

public class AuthService : IAuthService
{
    private const int Iteraciones = 100000;
    private const int TamanoSal = 16;
    private const int TamanoHash = 32;

    private readonly IConfiguration _configuration;

    public AuthService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerarHash(string password)
    {
        byte[] sal = RandomNumberGenerator.GetBytes(TamanoSal);

        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, sal, Iteraciones, HashAlgorithmName.SHA256, TamanoHash);

        return $"{Iteraciones}.{Convert.ToBase64String(sal)}.{Convert.ToBase64String(hash)}";
    }

    public bool VerificarPassword(string password, string hashGuardado)
    {
        string[] partes = hashGuardado.Split('.');

        if (partes.Length != 3)
        {
            return false;
        }

        int iteraciones = int.Parse(partes[0]);
        byte[] sal = Convert.FromBase64String(partes[1]);
        byte[] esperado = Convert.FromBase64String(partes[2]);

        byte[] calculado = Rfc2898DeriveBytes.Pbkdf2(password, sal, iteraciones, HashAlgorithmName.SHA256, esperado.Length);

        return CryptographicOperations.FixedTimeEquals(calculado, esperado);
    }

    public string GenerarToken(User usuario)
    {
        int minutos = int.Parse(_configuration["Jwt:MinutosDeExpiracion"]!);

        List<Claim> claims = new List<Claim>
        {
            new Claim(NombreClaim.UserId, usuario.UserId.ToString()),
            new Claim(ClaimTypes.Name, usuario.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if (usuario.RestaurantId != null)
        {
            claims.Add(new Claim(NombreClaim.RestaurantId, usuario.RestaurantId.Value.ToString()));
        }

        foreach (UserRole asignacion in usuario.UserRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, asignacion.Role.RoleName));
        }

        SymmetricSecurityKey clave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
        SigningCredentials credenciales = new SigningCredentials(clave, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(minutos),
            signingCredentials: credenciales);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
