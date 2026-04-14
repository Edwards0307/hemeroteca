using Microsoft.AspNetCore.Mvc;
using Hemeroteca.API.Models;
using Hemeroteca.API.Services.Interfaces;

namespace Hemeroteca.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LibrosController : ControllerBase
{
    private readonly ILibroService _libroService;

    public LibrosController(ILibroService libroService)
    {
        _libroService = libroService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? categoriaId, [FromQuery] string? buscar)
    {
        var libros = await _libroService.GetAllAsync(categoriaId, buscar);
        return Ok(libros);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var libro = await _libroService.GetByIdAsync(id);
        if (libro == null) return NotFound();
        return Ok(libro);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Libro libro)
    {
        var id = await _libroService.CreateAsync(libro);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Libro libro)
    {
        var actualizado = await _libroService.UpdateAsync(id, libro);
        if (!actualizado) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var eliminado = await _libroService.DeleteAsync(id);
        if (!eliminado) return NotFound();
        return NoContent();
    }

    [HttpPost("{id}/descargar")]
    public async Task<IActionResult> Descargar(int id)
    {
        var ruta = await _libroService.DescargarAsync(id);
        if (ruta == null) return NotFound();
        return Ok(new { ruta });
    }
}
