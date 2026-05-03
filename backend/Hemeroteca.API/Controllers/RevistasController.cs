using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Hemeroteca.API.Models;
using Hemeroteca.API.Services.Interfaces;

namespace Hemeroteca.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RevistasController : ControllerBase
{
    private readonly IRevistaService _revistaService;
    private readonly string _adminUsername;

    public RevistasController(IRevistaService revistaService, IConfiguration configuration)
    {
        _revistaService = revistaService;
        _adminUsername = configuration["AdminUsername"] ?? string.Empty;
    }

    private bool EsAdmin() =>
        User.Identity?.Name?.Equals(_adminUsername, StringComparison.OrdinalIgnoreCase) ?? false;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? categoriaId, [FromQuery] string? buscar)
    {
        var revistas = await _revistaService.GetAllAsync(categoriaId, buscar);
        return Ok(revistas);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var revista = await _revistaService.GetByIdAsync(id);
        if (revista == null) return NotFound();
        return Ok(revista);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] Revista revista)
    {
        if (!EsAdmin()) return Forbid();
        var id = await _revistaService.CreateAsync(revista);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, [FromBody] Revista revista)
    {
        if (!EsAdmin()) return Forbid();
        var actualizado = await _revistaService.UpdateAsync(id, revista);
        if (!actualizado) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        if (!EsAdmin()) return Forbid();
        var eliminado = await _revistaService.DeleteAsync(id);
        if (!eliminado) return NotFound();
        return NoContent();
    }

    [HttpPost("{id}/descargar")]
    public async Task<IActionResult> Descargar(int id)
    {
        var ruta = await _revistaService.DescargarAsync(id);
        if (ruta == null) return NotFound();
        return Ok(new { ruta });
    }
}
