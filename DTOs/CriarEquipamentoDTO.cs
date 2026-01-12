namespace APIArenaAuto.DTOs
{
    public class CriarEquipamentoDTO
    {
        public string NomeEquipamento { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
    }

    public class AtualizarQuantidadeDTO
    {
        public int Quantidade { get; set; }
    }
}