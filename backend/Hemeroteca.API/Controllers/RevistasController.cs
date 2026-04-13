using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hemeroteca.API.Data;
using Hemeroteca.API.Models;

namespace Hemeroteca.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RevistasController : ControllerBase
{
    private readonly HemerotecaContext _context;

    public RevistasController(HemerotecaContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? categoriaId, [FromQuery] string? buscar)
    {
        var query = _context.Revistas.Include(r => r.Categoria).AsQueryable();

        if (categoriaId.HasValue)
            query = query.Where(r => r.CategoriaId == categoriaId);

        if (!string.IsNullOrEmpty(buscar))
            query = query.Where(r => r.Titulo.Contains(buscar) ||
                                     r.Autor!.Contains(buscar) ||
                                     r.Descripcion!.Contains(buscar));

        var revistas = await query.OrderByDescending(r => r.FechaRegistro).ToListAsync();
        return Ok(revistas);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var revista = await _context.Revistas.Include(r => r.Categoria).FirstOrDefaultAsync(r => r.Id == id);
        if (revista == null) return NotFound();
        return Ok(revista);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Revista revista)
    {
        _context.Revistas.Add(revista);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = revista.Id }, revista);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Revista revista)
    {
        if (id != revista.Id) return BadRequest();
        _context.Entry(revista).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var revista = await _context.Revistas.FindAsync(id);
        if (revista == null) return NotFound();
        _context.Revistas.Remove(revista);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("{id}/descargar")]
    public async Task<IActionResult> Descargar(int id)
    {
        var revista = await _context.Revistas.FindAsync(id);
        if (revista == null) return NotFound();
        revista.TotalDescargas++;
        await _context.SaveChangesAsync();
        return Ok(new { ruta = revista.RutaArchivo, total = revista.TotalDescargas });
    }
}
