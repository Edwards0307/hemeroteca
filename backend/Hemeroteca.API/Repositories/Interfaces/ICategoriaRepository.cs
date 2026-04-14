using Hemeroteca.API.Models;

namespace Hemeroteca.API.Repositories.Interfaces;

public interface ICategoriaRepository
{
    Task<IEnumerable<Categoria>> GetAllAsync();
    Task<Categoria?> GetByIdAsync(int id);
    Task<int> CreateAsync(Categoria categoria);
    Task<bool> UpdateAsync(Categoria categoria);
    Task<bool> DeleteAsync(int id);
}
