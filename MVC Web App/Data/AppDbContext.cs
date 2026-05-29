using Microsoft.EntityFrameworkCore;
using MVC_ProyectoFinalPOO.Entities;

namespace MVC_ProyectoFinalPOO.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Partida> Partidas => Set<Partida>();
        public DbSet<Estadistica> Estadisticas => Set<Estadistica>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Nickname).IsUnique();
                entity.Property(e => e.Nickname).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PasswordHash).IsRequired();
            });

            modelBuilder.Entity<Partida>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NombreGanador).IsRequired().HasMaxLength(50);
                entity.HasOne(e => e.Usuario)
                      .WithMany(u => u.Partidas)
                      .HasForeignKey(e => e.UsuarioId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Estadistica>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Usuario)
                      .WithOne(u => u.Estadistica)
                      .HasForeignKey<Estadistica>(e => e.UsuarioId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
