namespace APIArenaAuto.DTOs
{
    public class LoginResponseDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string NivelAcesso { get; set; } = string.Empty; 
        public string Setor { get; set; } = string.Empty;
        public string Empresa { get; set; } = string.Empty;
    }
}