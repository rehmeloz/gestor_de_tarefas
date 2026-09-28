using gerenciador_de_tarefas.Models;
using Microsoft.EntityFrameworkCore;

namespace gerenciador_de_tarefas.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Tarefa> Tarefas { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>()
            .HasMany(u => u.Tarefas)
            .WithOne(t => t.Usuario)
            .HasForeignKey(t => t.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Usuario>()
            .HasMany(u => u.Categorias)
            .WithOne(c => c.Usuario)
            .HasForeignKey(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Categoria>()
            .HasMany(c => c.Tarefas)
            .WithOne(t => t.Categoria)
            .HasForeignKey(t => t.CategoriaId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
