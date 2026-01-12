namespace APIArenaAuto.DTOs
{
    public class ModeloEquipamentoDTO
    {
        public int Id { get; set; }
        public int EquipamentoId { get; set; }
        public string Modelo { get; set; } = string.Empty;
        public int Quantidade { get; set; }
    }
}
