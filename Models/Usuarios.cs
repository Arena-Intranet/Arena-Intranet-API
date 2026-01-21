using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIArenaAuto.Models
{
    [Table("usuarios")]
    public class Usuario
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("nome")]
        public string Nome { get; set; } = string.Empty;

        [Column("empresa")]
        public string? Empresa { get; set; }

        [Column("setor")]
        public string? Setor { get; set; }

        [Column("cargo")]
        public string? Cargo { get; set; }

        [Column("superioridade")]
        public string? superioridade { get; set; }

        [Column("data_nascimento")]
        public DateTime? DataNascimento { get; set; }

        [Column("data_admissao")]
        public DateTime? DataAdmissao { get; set; }

        [Column("usuario")]
        public string UsuarioLogin { get; set; } = string.Empty;

        [Column("telefone")]
        public string? Telefone { get; set; }

        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Column("foto")]
        public string? Foto { get; set; }

        [Column("nivel_acesso")]
        public string NivelAcesso { get; set; } = "COLABORADOR";

        [Column("senha_hash")]
        public string SenhaHash { get; set; } = string.Empty;

        [Column("cpf")]
        public string? Cpf { get; set; }
    }
}
