using System.ComponentModel.DataAnnotations.Schema;

namespace APIArenaAuto.Models
{
    public class Equipamento
    {
        public int Id { get; set; }

        [Column("nome_equipamento")]
        public string NomeEquipamento { get; set; } = string.Empty;

        [Column("categoria")]
        public string Categoria { get; set; } = string.Empty;

        public ICollection<ModeloEquipamento> Modelos { get; set; } = new List<ModeloEquipamento>();
    }
}
