namespace APIArenaAuto.DTOs
{
    public class EquipamentoListaDTO
    {
        public int Id { get; set; }
        public string Equipamento { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public int QtdMarcas { get; set; }
        public int QuantidadeTotal { get; set; }
    }
}
