using Hemeroteca.API.Models;
using Hemeroteca.API.Repositories.Interfaces;
using Hemeroteca.API.Services.Interfaces;

namespace Hemeroteca.API.Services;

public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _categoriaRepository;

    public CategoriaService(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    public async Task<IEnumerable<Categoria>> GetAllAsync()
    {
        return await _categoriaRepository.GetAllAsync();
    }

    public async Task<Categoria?> GetByIdAsync(int id)
    {
        return await _categoriaRepository.GetByIdAsync(id);
    }

    public async Task<int> CreateAsync(Categoria categoria)
    {
        if (string.IsNullOrWhiteSpace(categoria.Nombre))
            throw new ArgumentException("El nombre es obligatorio");

        return await _categoriaRepository.CreateAsync(categoria);
    }

    public async Task<bool> UpdateAsync(int id, Categoria categoria)
    {
        if (string.IsNullOrWhiteSpace(categoria.Nombre))
            throw new ArgumentException("El nombre es obligatorio");

        var existe = await _categoriaRepository.GetByIdAsync(id);
        if (existe == null) return false;

        categoria.Id = id;
        return await _categoriaRepository.UpdateAsync(categoria);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existe = await _categoriaRepository.GetByIdAsync(id);
        if (existe == null) return false;

        return await _categoriaRepository.DeleteAsync(id);
    }
}
