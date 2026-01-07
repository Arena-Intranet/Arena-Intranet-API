using System.ComponentModel.DataAnnotations.Schema;

namespace APIArenaAuto.Models
{
    [Table("Comunicados")] 
    public class Comunicado
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("titulo")]
        public string Titulo { get; set; } = string.Empty;

        [Column("categoria")]
        public string Categoria { get; set; } = string.Empty;

        [Column("urgencia")]
        public string Urgencia { get; set; } = string.Empty;

        [Column("conteudo")]
        public string Conteudo { get; set; } = string.Empty;

        [Column("data_postagem")]
        public DateTime DataPostagem { get; set; }
    }
}
