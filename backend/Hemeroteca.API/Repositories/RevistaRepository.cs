using Dapper;
using Hemeroteca.API.Common;
using Hemeroteca.API.Models;
using Hemeroteca.API.Repositories.Interfaces;

namespace Hemeroteca.API.Repositories;

public class RevistaRepository : IRevistaRepository
{
    private readonly IDbConnectionFactory _db;

    public RevistaRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Revista>> GetAllAsync(int? categoriaId, string? buscar)
    {
        using var connection = _db.CreateConnection();
        return await connection.QueryAsync<Revista, Categoria, Revista>(
            "SELECT r.*, c.* FROM \"Revistas\" r INNER JOIN \"Categorias\" c ON r.\"CategoriaId\" = c.\"Id\" WHERE (@CategoriaId IS NULL OR r.\"CategoriaId\" = @CategoriaId) AND (@Buscar IS NULL OR r.\"Titulo\" ILIKE @BuscarLike OR r.\"Autor\" ILIKE @BuscarLike) ORDER BY r.\"FechaRegistro\" DESC",
            (revista, categoria) => { revista.Categoria = categoria; return revista; },
            new { CategoriaId = categoriaId, Buscar = buscar, BuscarLike = $"%{buscar}%" },
            splitOn: "Id"
        );
    }

    public async Task<Revista?> GetByIdAsync(int id)
    {
        using var connection = _db.CreateConnection();
        var result = await connection.QueryAsync<Revista, Categoria, Revista>(
            "SELECT r.*, c.* FROM \"Revistas\" r INNER JOIN \"Categorias\" c ON r.\"CategoriaId\" = c.\"Id\" WHERE r.\"Id\" = @Id",
            (revista, categoria) => { revista.Categoria = categoria; return revista; },
            new { Id = id },
            splitOn: "Id"
        );
        return result.FirstOrDefault();
    }

    public async Task<int> CreateAsync(Revista revista)
    {
        using var connection = _db.CreateConnection();
        return await connection.ExecuteScalarAsync<int>(
            "INSERT INTO \"Revistas\" (\"Titulo\", \"Autor\", \"LugarPublicacion\", \"Idioma\", \"Ano\", \"Descripcion\", \"RutaArchivo\", \"TipoDocumento\", \"FechaPublicacion\", \"FechaRegistro\", \"TotalDescargas\", \"CategoriaId\") VALUES (@Titulo, @Autor, @LugarPublicacion, @Idioma, @Ano, @Descripcion, @RutaArchivo, @TipoDocumento, @FechaPublicacion, @FechaRegistro, 0, @CategoriaId) RETURNING \"Id\"",
            revista
        );
    }

    public async Task<bool> UpdateAsync(Revista revista)
    {
        using var connection = _db.CreateConnection();
        var rows = await connection.ExecuteAsync(
            "UPDATE \"Revistas\" SET \"Titulo\"=@Titulo, \"Autor\"=@Autor, \"LugarPublicacion\"=@LugarPublicacion, \"Idioma\"=@Idioma, \"Ano\"=@Ano, \"Descripcion\"=@Descripcion, \"RutaArchivo\"=@RutaArchivo, \"TipoDocumento\"=@TipoDocumento, \"FechaPublicacion\"=@FechaPublicacion, \"CategoriaId\"=@CategoriaId WHERE \"Id\"=@Id",
            revista
        );
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _db.CreateConnection();
        var rows = await connection.ExecuteAsync(
            "DELETE FROM \"Revistas\" WHERE \"Id\" = @Id",
            new { Id = id }
        );
        return rows > 0;
    }

    public async Task<bool> IncrementarDescargasAsync(int id)
    {
        using var connection = _db.CreateConnection();
        var rows = await connection.ExecuteAsync(
            "UPDATE \"Revistas\" SET \"TotalDescargas\" = \"TotalDescargas\" + 1 WHERE \"Id\" = @Id",
            new { Id = id }
        );
        return rows > 0;
    }
}
