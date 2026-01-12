using System.ComponentModel.DataAnnotations.Schema;

namespace APIArenaAuto.Models
{
    public class Ouvidoria
    {
        public int Id { get; set; }

        [Column("data_envio")]
        public DateTime DataEnvio { get; set; }

        public string? Categoria { get; set; }

        public string? Assunto { get; set; }

        [Column("mensagem")]
        public string? Mensagem { get; set; }

        public bool? Anonimo { get; set; }

        [Column("autor_id")]
        public int? AutorId { get; set; }

        [Column("autor_nome")]
        public string? AutorNome { get; set; }
    }
}
