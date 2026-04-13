namespace Hemeroteca.API.Models;

public class Libro
{
    public int Id { get; set; }
    public string? Codigo { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Autor { get; set; }
    public string? Editorial { get; set; }
    public string? Idioma { get; set; }
    public int? Paginas { get; set; }
    public int? Ano { get; set; }
    public string? Descripcion { get; set; }
    public string? RutaImagen { get; set; }
    public string? RutaArchivo { get; set; }
    public string? TipoDocumento { get; set; }
    public DateTime? FechaPublicacion { get; set; }
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
    public int TotalDescargas { get; set; } = 0;
    public int CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }
}
