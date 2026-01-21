namespace APIArenaAuto.DTOs
{
    public class UsuarioCreateDto
    {
        public string Nome { get; set; } = string.Empty;
        public string? Empresa { get; set; }
        public string? Setor { get; set; }
        public string? Cargo { get; set; }

        public string? DataNascimento { get; set; }
        public string? DataAdmissao { get; set; }

        public string? NivelAcesso { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string? Telefone { get; set; }
        public string Email { get; set; } = string.Empty;
        public string superioridade { get; set; } = string.Empty;
        public string? Foto { get; set; }
        public string? Cpf { get; set; }
        public string? Senha { get; set; }
    }
}
