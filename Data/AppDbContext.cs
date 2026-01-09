using APIArenaAuto.Models;
using Microsoft.EntityFrameworkCore;

namespace APIArenaAuto.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Treinamento> Treinamentos { get; set; }
        public DbSet<Comunicado> Comunicados { get; set; }
        public DbSet<Atendimento> Atendimentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>()
                .ToTable("usuarios");

            modelBuilder.Entity<Treinamento>()
                .ToTable("treinamentos");

            modelBuilder.Entity<Comunicado>()
                .ToTable("comunicados");

            modelBuilder.Entity<Atendimento>()
                .ToTable("atendimentos");
        }
    }
}
