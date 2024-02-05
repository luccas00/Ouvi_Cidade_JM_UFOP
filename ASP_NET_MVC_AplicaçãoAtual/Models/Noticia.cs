using System.ComponentModel.DataAnnotations;

namespace OuviCidadeV3.Models
{
    public class Noticia
    {
        public Admin Proprietario { get; set; }
        public string Titulo { get; set; }
        public string Texto { get; set; }
        public DateTime DataCriacao { get; set; }
        [Key]
        public string ID { get; set; }
        public Secretaria? Secretaria { get; set; }
    }
}
