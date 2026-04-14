using Dapper;
using Hemeroteca.API.Common;
using Hemeroteca.API.Models;
using Hemeroteca.API.Repositories.Interfaces;

namespace Hemeroteca.API.Repositories;

public class LibroRepository : ILibroRepository
{
    private readonly IDbConnectionFactory _db;

    public LibroRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Libro>> GetAllAsync(int? categoriaId, string? buscar)
    {
        using var connection = _db.CreateConnection();
        return await connection.QueryAsync<Libro, Categoria, Libro>(
            "SELECT l.*, c.* FROM \"Libros\" l INNER JOIN \"Categorias\" c ON l.\"CategoriaId\" = c.\"Id\" WHERE (@CategoriaId IS NULL OR l.\"CategoriaId\" = @CategoriaId) AND (@Buscar IS NULL OR l.\"Titulo\" ILIKE @BuscarLike OR l.\"Autor\" ILIKE @BuscarLike) ORDER BY l.\"FechaRegistro\" DESC",
            (libro, categoria) => { libro.Categoria = categoria; return libro; },
            new { CategoriaId = categoriaId, Buscar = buscar, BuscarLike = $"%{buscar}%" },
            splitOn: "Id"
        );
    }

    public async Task<Libro?> GetByIdAsync(int id)
    {
        using var connection = _db.CreateConnection();
        var result = await connection.QueryAsync<Libro, Categoria, Libro>(
            "SELECT l.*, c.* FROM \"Libros\" l INNER JOIN \"Categorias\" c ON l.\"CategoriaId\" = c.\"Id\" WHERE l.\"Id\" = @Id",
            (libro, categoria) => { libro.Categoria = categoria; return libro; },
            new { Id = id },
            splitOn: "Id"
        );
        return result.FirstOrDefault();
    }

    public async Task<int> CreateAsync(Libro libro)
    {
        using var connection = _db.CreateConnection();
        return await connection.ExecuteScalarAsync<int>(
            "INSERT INTO \"Libros\" (\"Codigo\", \"Titulo\", \"Autor\", \"Editorial\", \"Idioma\", \"Paginas\", \"Ano\", \"Descripcion\", \"RutaImagen\", \"RutaArchivo\", \"TipoDocumento\", \"FechaPublicacion\", \"FechaRegistro\", \"TotalDescargas\", \"CategoriaId\") VALUES (@Codigo, @Titulo, @Autor, @Editorial, @Idioma, @Paginas, @Ano, @Descripcion, @RutaImagen, @RutaArchivo, @TipoDocumento, @FechaPublicacion, @FechaRegistro, 0, @CategoriaId) RETURNING \"Id\"",
            libro
        );
    }

    public async Task<bool> UpdateAsync(Libro libro)
    {
        using var connection = _db.CreateConnection();
        var rows = await connection.ExecuteAsync(
            "UPDATE \"Libros\" SET \"Codigo\"=@Codigo, \"Titulo\"=@Titulo, \"Autor\"=@Autor, \"Editorial\"=@Editorial, \"Idioma\"=@Idioma, \"Paginas\"=@Paginas, \"Ano\"=@Ano, \"Descripcion\"=@Descripcion, \"RutaImagen\"=@RutaImagen, \"RutaArchivo\"=@RutaArchivo, \"TipoDocumento\"=@TipoDocumento, \"FechaPublicacion\"=@FechaPublicacion, \"CategoriaId\"=@CategoriaId WHERE \"Id\"=@Id",
            libro
        );
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = _db.CreateConnection();
        var rows = await connection.ExecuteAsync(
            "DELETE FROM \"Libros\" WHERE \"Id\" = @Id",
            new { Id = id }
        );
        return rows > 0;
    }

    public async Task<bool> IncrementarDescargasAsync(int id)
    {
        using var connection = _db.CreateConnection();
        var rows = await connection.ExecuteAsync(
            "UPDATE \"Libros\" SET \"TotalDescargas\" = \"TotalDescargas\" + 1 WHERE \"Id\" = @Id",
            new { Id = id }
        );
        return rows > 0;
    }
}
