using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Hemeroteca.API.Data;
using Hemeroteca.API.Models;

namespace Hemeroteca.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly HemerotecaContext _context;
    private readonly IConfiguration _config;

    public AuthController(HemerotecaContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var hash = HashPassword(request.Password);
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Username == request.Username && u.PasswordHash == hash);

        if (usuario == null) return Unauthorized(new { message = "Credenciales incorrectas" });

        var token = GenerarToken(usuario);
        return Ok(new { token });
    }

    [HttpPost("registro")]
    public async Task<IActionResult> Registro([FromBody] LoginRequest request)
    {
        if (await _context.Usuarios.AnyAsync(u => u.Username == request.Username))
            return BadRequest(new { message = "El usuario ya existe" });

        var usuario = new Usuario
        {
            Username = request.Username,
            PasswordHash = HashPassword(request.Password)
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Usuario creado correctamente" });
    }

    private string GenerarToken(Usuario usuario)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _config["Jwt:Key"] ?? "hemeroteca_secret_key_2024_segura"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Username)
        };
        var token = new JwtSecurityToken(claims: claims, expires: DateTime.UtcNow.AddHours(8), signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes).ToLower();
    }
}

public record LoginRequest(string Username, string Password);
