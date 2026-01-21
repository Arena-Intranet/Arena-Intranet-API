using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIArenaAuto.Models
{
    [Table("Organograma")]
    public class Organograma
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("setor")]
        public string Setor { get; set; } = string.Empty;

        [Column("gestor_id")]
        public int? GestorId { get; set; }

        [ForeignKey("GestorId")]
        public Usuario? Gestor { get; set; }

        [Column("liderado_id")]
        public int LideradoId { get; set; }

        [ForeignKey("LideradoId")]
        public Usuario Liderado { get; set; } = null!;
    }
}
