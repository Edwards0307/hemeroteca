using Hemeroteca.API.Models;

namespace Hemeroteca.API.Repositories.Interfaces;

public interface IRevistaRepository
{
    Task<IEnumerable<Revista>> GetAllAsync(int? categoriaId, string? buscar);
    Task<Revista?> GetByIdAsync(int id);
    Task<int> CreateAsync(Revista revista);
    Task<bool> UpdateAsync(Revista revista);
    Task<bool> DeleteAsync(int id);
    Task<bool> IncrementarDescargasAsync(int id);
}
