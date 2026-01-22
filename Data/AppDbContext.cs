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

            // Mapeamento das Tabelas (Exatamente como no Banco SQL)
            modelBuilder.Entity<Usuario>().ToTable("Usuarios");
            modelBuilder.Entity<Treinamento>().ToTable("Treinamentos");
            modelBuilder.Entity<Comunicado>().ToTable("Comunicados");
            modelBuilder.Entity<Atendimento>().ToTable("Atendimentos");
            modelBuilder.Entity<Ouvidoria>().ToTable("Ouvidoria");
            modelBuilder.Entity<Equipamento>().ToTable("Equipamentos");
            modelBuilder.Entity<ModeloEquipamento>().ToTable("ModelosEquipamento");
            modelBuilder.Entity<Organograma>().ToTable("Organograma");

            // Relacionamento Equipamento -> Modelos
            modelBuilder.Entity<ModeloEquipamento>()
                .HasOne(m => m.Equipamento)
                .WithMany(e => e.Modelos)
                .HasForeignKey(m => m.EquipamentoId) // Verifique se na sua Model está EquipamentoId ou id_equipamento
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamentos do Organograma
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