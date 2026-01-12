using System.ComponentModel.DataAnnotations.Schema;

namespace APIArenaAuto.Models
{
    public class ModeloEquipamento
    {
        public int Id { get; set; }

        [Column("id_equipamento")]
        public int EquipamentoId { get; set; }

        [Column("modelo")]
        public string Modelo { get; set; } = string.Empty;

        [Column("quantidade")]
        public int Quantidade { get; set; }

        public Equipamento Equipamento { get; set; } = null!;
    }
}
