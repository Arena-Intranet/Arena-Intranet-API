namespace APIArenaAuto.DTOs
{
    public class OrganogramaDto
    {
        public int UsuarioId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Cargo { get; set; } = string.Empty;
        public string Setor { get; set; } = string.Empty;
        public int? GestorId { get; set; }
        public List<OrganogramaDto> Subordinados { get; set; } = new();
    }
 
}
