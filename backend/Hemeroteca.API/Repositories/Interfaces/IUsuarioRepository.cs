using Hemeroteca.API.Models;

namespace Hemeroteca.API.Repositories.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByUsernameAsync(string username);
    Task<bool> ExistsAsync(string username);
    Task<int> CreateAsync(Usuario usuario);
}
