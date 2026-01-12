using System.ComponentModel.DataAnnotations.Schema;

namespace APIArenaAuto.DTOs
{
    public class OuvidoriaCreateDto
    {
        public string? Categoria { get; set; }
        public string? Assunto { get; set; }
        public string? Mensagem { get; set; }
        public bool? Anonimo { get; set; }
        public int? AutorId { get; set; }
        public string? AutorNome { get; set; }
    }
}
