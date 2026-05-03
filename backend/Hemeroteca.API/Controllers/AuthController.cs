using Microsoft.AspNetCore.Mvc;
using Hemeroteca.API.Services.Interfaces;

namespace Hemeroteca.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly string _adminUsername;

    public AuthController(IAuthService authService, IConfiguration configuration)
    {
        _authService = authService;
        _adminUsername = configuration["AdminUsername"] ?? string.Empty;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var token = await _authService.LoginAsync(request.Username, request.Password);
        if (token == null) return Unauthorized(new { message = "Credenciales incorrectas" });
        var isAdmin = request.Username.Equals(_adminUsername, StringComparison.OrdinalIgnoreCase);
        return Ok(new { token, isAdmin });
    }

    [HttpPost("registro")]
    public async Task<IActionResult> Registro([FromBody] LoginRequest request)
    {
        var creado = await _authService.RegistroAsync(request.Username, request.Password);
        if (!creado) return BadRequest(new { message = "El usuario ya existe" });
        return Ok(new { message = "Usuario creado correctamente" });
    }
}

public record LoginRequest(string Username, string Password);
