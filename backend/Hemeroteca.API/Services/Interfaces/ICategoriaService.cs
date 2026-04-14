using Hemeroteca.API.Models;

namespace Hemeroteca.API.Services.Interfaces;

public interface ICategoriaService
{
    Task<IEnumerable<Categoria>> GetAllAsync();
    Task<Categoria?> GetByIdAsync(int id);
    Task<int> CreateAsync(Categoria categoria);
    Task<bool> UpdateAsync(int id, Categoria categoria);
    Task<bool> DeleteAsync(int id);
}
