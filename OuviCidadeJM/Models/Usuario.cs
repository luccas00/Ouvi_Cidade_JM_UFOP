using System.ComponentModel.DataAnnotations;

namespace OuviCidadeJM.Models
{
    public class Usuario
    {
        public string? Nome { get; set; }
        public string? Telefone { get; set; }
        [Key]
        public string ID { get; set; }

    }
}
