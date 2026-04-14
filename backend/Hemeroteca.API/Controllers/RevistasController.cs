using Microsoft.AspNetCore.Mvc;
using Hemeroteca.API.Models;
using Hemeroteca.API.Services.Interfaces;

namespace Hemeroteca.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RevistasController : ControllerBase
{
    private readonly IRevistaService _revistaService;

    public RevistasController(IRevistaService revistaService)
    {
        _revistaService = revistaService;
    }

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
    public async Task<IActionResult> Create([FromBody] Revista revista)
    {
        var id = await _revistaService.CreateAsync(revista);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Revista revista)
    {
        var actualizado = await _revistaService.UpdateAsync(id, revista);
        if (!actualizado) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
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
