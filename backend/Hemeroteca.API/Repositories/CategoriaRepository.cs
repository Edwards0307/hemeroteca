using Dapper;
using Hemeroteca.API.Common;
using Hemeroteca.API.Models;
using Hemeroteca.API.Repositories.Interfaces;

namespace Hemeroteca.API.Repositories;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly IDbConnectionFactory _db;

    public CategoriaRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Categoria>> GetAllAsync()
    {
        using var connection = _db.CreateConnection();
        return await connection.QueryAsync<Categoria>(
            "SELECT * FROM \"Categorias\" ORDER BY \"Nombre\""
        );
    }

    public async Task<Categoria?> GetByIdAsync(int id)
    {
        using var connection = _db.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Categoria>(
            "SELECT * FROM \"Categorias\" WHERE \"Id\" = @Id",
            new { Id = id }
        );
    }

    public async Task<int> CreateAsync(Categoria categoria)
    {
        using var connection = _db.CreateConnection();
        return await connection.ExecuteScalarAsync<int>(
            "INSERT INTO \"Categorias\" (\"Nombre\") VALUES (@Nombre) RETURNING \"Id\"",
            categoria
        );
    }

    public async Task<bool> UpdateAsync(Categoria categoria)
    {
        using var connection = _db.CreateConnection();
        var rows = await connection.ExecuteAsync(
            "UPDATE \"Categorias\" SET \"Nombre\" = @Nombre WHERE \"Id\" = @Id",
            categoria
        );
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _db.CreateConnection();
        var rows = await connection.ExecuteAsync(
            "DELETE FROM \"Categorias\" WHERE \"Id\" = @Id",
            new { Id = id }
        );
        return rows > 0;
    }
}
