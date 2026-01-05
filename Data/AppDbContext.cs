
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

        public DbSet<Treinamento> Treinamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Treinamento>()
                .ToTable("Treinamentos"); // nome exato da tabela
        }
    }
}
