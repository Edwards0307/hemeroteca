using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hemeroteca.API.Data;
using Hemeroteca.API.Models;

namespace Hemeroteca.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LibrosController : ControllerBase
{
    private readonly HemerotecaContext _context;

    public LibrosController(HemerotecaContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? categoriaId, [FromQuery] string? buscar)
    {
        var query = _context.Libros.Include(l => l.Categoria).AsQueryable();

        if (categoriaId.HasValue)
            query = query.Where(l => l.CategoriaId == categoriaId);

        if (!string.IsNullOrEmpty(buscar))
            query = query.Where(l => l.Titulo.Contains(buscar) ||
                                     l.Autor!.Contains(buscar) ||
                                     l.Descripcion!.Contains(buscar));

        var libros = await query.OrderByDescending(l => l.FechaRegistro).ToListAsync();
        return Ok(libros);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var libro = await _context.Libros.Include(l => l.Categoria).FirstOrDefaultAsync(l => l.Id == id);
        if (libro == null) return NotFound();
        return Ok(libro);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Libro libro)
    {
        _context.Libros.Add(libro);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = libro.Id }, libro);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Libro libro)
    {
        if (id != libro.Id) return BadRequest();
        _context.Entry(libro).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var libro = await _context.Libros.FindAsync(id);
        if (libro == null) return NotFound();
        _context.Libros.Remove(libro);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("{id}/descargar")]
    public async Task<IActionResult> Descargar(int id)
    {
        var libro = await _context.Libros.FindAsync(id);
        if (libro == null) return NotFound();
        libro.TotalDescargas++;
        await _context.SaveChangesAsync();
        return Ok(new { ruta = libro.RutaArchivo, total = libro.TotalDescargas });
    }
}
