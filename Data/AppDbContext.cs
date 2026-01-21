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
        public DbSet<Ouvidoria> Ouvidoria { get; set; }
        public DbSet<Equipamento> Equipamentos { get; set; }
        public DbSet<ModeloEquipamento> ModelosEquipamento { get; set; }
        public DbSet<Organograma> Organograma { get; set; }

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

            modelBuilder.Entity<Ouvidoria>()
                .ToTable("ouvidoria");


            modelBuilder.Entity<Equipamento>()
                .ToTable("equipamentos");

            modelBuilder.Entity<ModeloEquipamento>()
                .ToTable("ModelosEquipamento");

            modelBuilder.Entity<ModeloEquipamento>()
                .HasOne(m => m.Equipamento)
                .WithMany(e => e.Modelos)
                .HasForeignKey(m => m.EquipamentoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Organograma>()
               .HasOne(o => o.Gestor)
               .WithMany()
               .HasForeignKey(o => o.GestorId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Organograma>()
                .HasOne(o => o.Liderado)
                .WithMany()
                .HasForeignKey(o => o.LideradoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}