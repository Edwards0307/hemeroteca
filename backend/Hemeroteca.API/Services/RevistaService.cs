using Hemeroteca.API.Models;
using Hemeroteca.API.Repositories.Interfaces;
using Hemeroteca.API.Services.Interfaces;

namespace Hemeroteca.API.Services;

public class RevistaService : IRevistaService
{
    private readonly IRevistaRepository _revistaRepository;

    public RevistaService(IRevistaRepository revistaRepository)
    {
        _revistaRepository = revistaRepository;
    }

    public async Task<IEnumerable<Revista>> GetAllAsync(int? categoriaId, string? buscar)
    {
        return await _revistaRepository.GetAllAsync(categoriaId, buscar);
    }

    public async Task<Revista?> GetByIdAsync(int id)
    {
        return await _revistaRepository.GetByIdAsync(id);
    }

    public async Task<int> CreateAsync(Revista revista)
    {
        if (string.IsNullOrWhiteSpace(revista.Titulo))
            throw new ArgumentException("El título es obligatorio");

        revista.FechaRegistro = DateTime.UtcNow;
        return await _revistaRepository.CreateAsync(revista);
    }

    public async Task<bool> UpdateAsync(int id, Revista revista)
    {
        if (string.IsNullOrWhiteSpace(revista.Titulo))
            throw new ArgumentException("El título es obligatorio");

        var existe = await _revistaRepository.GetByIdAsync(id);
        if (existe == null) return false;

        revista.Id = id;
        return await _revistaRepository.UpdateAsync(revista);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existe = await _revistaRepository.GetByIdAsync(id);
        if (existe == null) return false;

        return await _revistaRepository.DeleteAsync(id);
    }

    public async Task<string?> DescargarAsync(int id)
    {
        var revista = await _revistaRepository.GetByIdAsync(id);
        if (revista == null) return null;

        await _revistaRepository.IncrementarDescargasAsync(id);
        return revista.RutaArchivo;
    }
}
