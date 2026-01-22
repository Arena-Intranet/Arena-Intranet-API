namespace APIArenaAuto.Models
{
    public class Ouvidoria
    {
        public int Id { get; set; }
        public DateTime DataEnvio { get; set; }
        public string? Categoria { get; set; }
        public string? Assunto { get; set; }
        public string? Mensagem { get; set; }
        public bool? Anonimo { get; set; }
        public int? AutorId { get; set; }
        public string? AutorNome { get; set; }
    }
}