using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Hemeroteca.API.Models;
using Hemeroteca.API.Repositories.Interfaces;
using Hemeroteca.API.Services.Interfaces;

namespace Hemeroteca.API.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IConfiguration _config;

    public AuthService(IUsuarioRepository usuarioRepository, IConfiguration config)
    {
        _usuarioRepository = usuarioRepository;
        _config = config;
    }

    public async Task<string?> LoginAsync(string username, string password)
    {
        var hash = HashPassword(password);
        var usuario = await _usuarioRepository.GetByUsernameAsync(username);

        if (usuario == null || usuario.PasswordHash != hash)
            return null;

        return GenerarToken(usuario);
    }

    public async Task<bool> RegistroAsync(string username, string password)
    {
        if (await _usuarioRepository.ExistsAsync(username))
            return false;

        var usuario = new Usuario
        {
            Username = username,
            PasswordHash = HashPassword(password),
            FechaCreacion = DateTime.UtcNow
        };

        var id = await _usuarioRepository.CreateAsync(usuario);
        return id > 0;
    }

    private string GenerarToken(Usuario usuario)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _config["Jwt:Key"] ?? "hemeroteca_secret_key_2024_segura"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Username)
        };
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes).ToLower();
    }
}
