using Hemeroteca.API.Models;
using Hemeroteca.API.Repositories.Interfaces;
using Hemeroteca.API.Services.Interfaces;

namespace Hemeroteca.API.Services;

public class LibroService : ILibroService
{
    private readonly ILibroRepository _libroRepository;

    public LibroService(ILibroRepository libroRepository)
    {
        _libroRepository = libroRepository;
    }

    public async Task<IEnumerable<Libro>> GetAllAsync(int? categoriaId, string? buscar)
    {
        return await _libroRepository.GetAllAsync(categoriaId, buscar);
    }

    public async Task<Libro?> GetByIdAsync(int id)
    {
        return await _libroRepository.GetByIdAsync(id);
    }

    public async Task<int> CreateAsync(Libro libro)
    {
        if (string.IsNullOrWhiteSpace(libro.Titulo))
            throw new ArgumentException("El título es obligatorio");

        libro.FechaRegistro = DateTime.UtcNow;
        return await _libroRepository.CreateAsync(libro);
    }

    public async Task<bool> UpdateAsync(int id, Libro libro)
    {
        if (string.IsNullOrWhiteSpace(libro.Titulo))
            throw new ArgumentException("El título es obligatorio");

        var existe = await _libroRepository.GetByIdAsync(id);
        if (existe == null) return false;

        libro.Id = id;
        return await _libroRepository.UpdateAsync(libro);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existe = await _libroRepository.GetByIdAsync(id);
        if (existe == null) return false;

        return await _libroRepository.DeleteAsync(id);
    }

    public async Task<string?> DescargarAsync(int id)
    {
        var libro = await _libroRepository.GetByIdAsync(id);
        if (libro == null) return null;

        await _libroRepository.IncrementarDescargasAsync(id);
        return libro.RutaArchivo;
    }
}
