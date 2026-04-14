using Hemeroteca.API.Models;

namespace Hemeroteca.API.Repositories.Interfaces;

public interface ILibroRepository
{
    Task<IEnumerable<Libro>> GetAllAsync(int? categoriaId, string? buscar);
    Task<Libro?> GetByIdAsync(int id);
    Task<int> CreateAsync(Libro libro);
    Task<bool> UpdateAsync(Libro libro);
    Task<bool> DeleteAsync(int id);
    Task<bool> IncrementarDescargasAsync(int id);
}
