using System.ComponentModel.DataAnnotations;

namespace OuviCidadeJM.Models
{
    public class Secretaria
    {
        [Key]
        public string Id { get; set; }
        public string Nome { get; set; }
    }
}
