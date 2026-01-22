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

            // 1. Mapeamento dos nomes das Tabelas
            modelBuilder.Entity<Usuario>().ToTable("Usuarios");
            modelBuilder.Entity<Treinamento>().ToTable("Treinamentos");
            modelBuilder.Entity<Comunicado>().ToTable("Comunicados");
            modelBuilder.Entity<Atendimento>().ToTable("Atendimentos");
            modelBuilder.Entity<Ouvidoria>().ToTable("Ouvidoria");
            modelBuilder.Entity<Equipamento>().ToTable("Equipamentos");
            modelBuilder.Entity<ModeloEquipamento>().ToTable("ModelosEquipamento");
            modelBuilder.Entity<Organograma>().ToTable("Organograma");

            // 2. Lógica Global: Converte automaticamente TODAS as propriedades para minúsculo
            // Isso resolve o erro 'column "Id" does not exist' em todas as rotas de uma vez.
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entity.GetProperties())
                {
                    // Se você não definiu um nome de coluna manualmente, ele força minúsculo
                    property.SetColumnName(property.Name.ToLower());
                }
            }

            // 3. Configurações específicas que fogem do padrão minúsculo simples
            modelBuilder.Entity<Ouvidoria>(entity =>
            {
                // Força o tipo boolean no Postgres para evitar o erro de 'bit'
                entity.Property(e => e.Anonimo).HasColumnType("boolean");

                // Mapeamentos com snake_case (com underline) precisam ser manuais
                entity.Property(e => e.DataEnvio).HasColumnName("data_envio");
                entity.Property(e => e.AutorId).HasColumnName("autor_id");
                entity.Property(e => e.AutorNome).HasColumnName("autor_nome");
            });

            modelBuilder.Entity<Atendimento>(entity =>
            {
                // Adicione aqui se houver colunas com underline no banco de Atendimentos
                // Exemplo: entity.Property(e => e.DataAbertura).HasColumnName("data_abertura");
            });

            // 4. Relacionamentos
            modelBuilder.Entity<ModeloEquipamento>()
                .HasOne(m => m.Equipamento)
                .WithMany(e => e.Modelos)
                .HasForeignKey(m => m.EquipamentoId);

            modelBuilder.Entity<ModeloEquipamento>()
                .Property(m => m.EquipamentoId)
                .HasColumnName("id_equipamento");

            modelBuilder.Entity<Organograma>(entity =>
            {
                entity.HasOne(o => o.Gestor).WithMany().HasForeignKey(o => o.GestorId);
                entity.Property(o => o.GestorId).HasColumnName("gestor_id");

                entity.HasOne(o => o.Liderado).WithMany().HasForeignKey(o => o.LideradoId);
                entity.Property(o => o.LideradoId).HasColumnName("liderado_id");
            });
        }
    }
}