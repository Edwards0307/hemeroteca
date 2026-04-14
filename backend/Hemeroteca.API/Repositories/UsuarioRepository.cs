using Dapper;
using Hemeroteca.API.Common;
using Hemeroteca.API.Models;
using Hemeroteca.API.Repositories.Interfaces;

namespace Hemeroteca.API.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly IDbConnectionFactory _db;

    public UsuarioRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    public async Task<Usuario?> GetByUsernameAsync(string username)
    {
        using var connection = _db.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Usuario>(
            "SELECT * FROM \"Usuarios\" WHERE \"Username\" = @Username",
            new { Username = username }
        );
    }

    public async Task<bool> ExistsAsync(string username)
    {
        using var connection = _db.CreateConnection();
        return await connection.ExecuteScalarAsync<bool>(
            "SELECT EXISTS(SELECT 1 FROM \"Usuarios\" WHERE \"Username\" = @Username)",
            new { Username = username }
        );
    }

    public async Task<int> CreateAsync(Usuario usuario)
    {
        using var connection = _db.CreateConnection();
        return await connection.ExecuteScalarAsync<int>(
            "INSERT INTO \"Usuarios\" (\"Username\", \"PasswordHash\", \"FechaCreacion\") VALUES (@Username, @PasswordHash, @FechaCreacion) RETURNING \"Id\"",
            usuario
        );
    }
}
