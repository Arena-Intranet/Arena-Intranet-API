namespace APIArenaAuto.Models
{
    public class Treinamento
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Instrutor { get; set; } = string.Empty;
        public string ImgVideo { get; set; } = string.Empty;
        public string VideoURL { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
    }
}
