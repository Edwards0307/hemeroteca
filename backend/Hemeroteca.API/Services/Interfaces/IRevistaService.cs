using Hemeroteca.API.Models;

namespace Hemeroteca.API.Services.Interfaces;

public interface IRevistaService
{
    Task<IEnumerable<Revista>> GetAllAsync(int? categoriaId, string? buscar);
    Task<Revista?> GetByIdAsync(int id);
    Task<int> CreateAsync(Revista revista);
    Task<bool> UpdateAsync(int id, Revista revista);
    Task<bool> DeleteAsync(int id);
    Task<string?> DescargarAsync(int id);
}
