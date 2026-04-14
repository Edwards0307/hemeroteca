using Microsoft.AspNetCore.Mvc;
using Hemeroteca.API.Models;
using Hemeroteca.API.Services.Interfaces;

namespace Hemeroteca.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;

    public CategoriasController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categorias = await _categoriaService.GetAllAsync();
        return Ok(categorias);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var categoria = await _categoriaService.GetByIdAsync(id);
        if (categoria == null) return NotFound();
        return Ok(categoria);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Categoria categoria)
    {
        var id = await _categoriaService.CreateAsync(categoria);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Categoria categoria)
    {
        var actualizado = await _categoriaService.UpdateAsync(id, categoria);
        if (!actualizado) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var eliminado = await _categoriaService.DeleteAsync(id);
        if (!eliminado) return NotFound();
        return NoContent();
    }
}
