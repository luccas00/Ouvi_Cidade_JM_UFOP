using System.ComponentModel.DataAnnotations;

namespace OuviCidadeJM.Models
{
    public class Noticia
    {
        public string? Proprietario { get; set; }
        public string? Detalhes { get; set; }
        public string? Resumo { get; set; }
        public string? DataCriacao { get; set; }
        [Key]
        public string ID { get; set; }
    }
}
