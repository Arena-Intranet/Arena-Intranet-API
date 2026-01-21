using System.ComponentModel.DataAnnotations.Schema;

namespace APIArenaAuto.Models
{
    public class Atendimento
    {
        public int Id { get; set; }
        public string Numero { get; set; } = string.Empty;

        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        [Column("nome")]
        public Usuario Usuario { get; set; }       

        public string Titulo { get; set; } = string.Empty;
        public string Setor { get; set; } = string.Empty;

        [Column("empresa")]
        public string Empresa { get; set; } = string.Empty;
        public string Mensagem { get; set; } = string.Empty;
        public string Status { get; set; } = "Pendente";

        [Column("data_abertura")]
        public DateTime DataAbertura { get; set; }

        [Column("data_termino")]
        public DateTime? DataTermino { get; set; }
    }

}
