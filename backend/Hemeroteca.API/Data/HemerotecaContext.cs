using Microsoft.EntityFrameworkCore;
using Hemeroteca.API.Models;

namespace Hemeroteca.API.Data;

public class HemerotecaContext : DbContext
{
    public HemerotecaContext(DbContextOptions<HemerotecaContext> options) : base(options) { }

    public DbSet<Libro> Libros { get; set; }
    public DbSet<Revista> Revistas { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>().HasData(
            new Categoria { Id = 1, Nombre = "Ciencia" },
            new Categoria { Id = 2, Nombre = "Historia" },
            new Categoria { Id = 3, Nombre = "Tecnología" },
            new Categoria { Id = 4, Nombre = "Arte" },
            new Categoria { Id = 5, Nombre = "Literatura" }
        );
    }
}
