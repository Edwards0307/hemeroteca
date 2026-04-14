using Hemeroteca.API.Models;

namespace Hemeroteca.API.Services.Interfaces;

public interface ILibroService
{
    Task<IEnumerable<Libro>> GetAllAsync(int? categoriaId, string? buscar);
    Task<Libro?> GetByIdAsync(int id);
    Task<int> CreateAsync(Libro libro);
    Task<bool> UpdateAsync(int id, Libro libro);
    Task<bool> DeleteAsync(int id);
    Task<string?> DescargarAsync(int id);
}
