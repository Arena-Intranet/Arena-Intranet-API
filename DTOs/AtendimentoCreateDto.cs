using System.ComponentModel.DataAnnotations;

namespace APIArenaAuto.DTOs
{
    public class AtendimentoCreateDto
    {
        [Required(ErrorMessage = "O número é obrigatório")]
        public string Numero { get; set; } = string.Empty;

        [Required(ErrorMessage = "O nome é obrigatório")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O título é obrigatório")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O setor é obrigatório")]
        public string Setor { get; set; } = string.Empty;

        [Required(ErrorMessage = "O setor é obrigatório")]
        public string Empresa { get; set; } = string.Empty;

        [Required(ErrorMessage = "A mensagem não pode estar vazia")]
        public string Mensagem { get; set; } = string.Empty;
    }
}