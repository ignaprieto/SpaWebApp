using Microsoft.EntityFrameworkCore;
using SpaWebApp.Models;

namespace SpaWebApp.Data
{
    public class SpaContext : DbContext
    {
        public SpaContext(DbContextOptions<SpaContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Turno> Turnos { get; set; }
        public DbSet<Consulta> Consultas { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Turno>()
                .HasOne(t => t.Usuario)
                .WithMany()
                .HasForeignKey(t => t.UsuarioID)
                .OnDelete(DeleteBehavior.Restrict); // Configura la relación para evitar eliminación en cascada
        }
    }
}
