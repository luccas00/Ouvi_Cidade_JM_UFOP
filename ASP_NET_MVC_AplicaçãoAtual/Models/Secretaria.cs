using System.ComponentModel.DataAnnotations;

namespace OuviCidadeV3.Models
{
    public class Secretaria
    {
        [Key]
        public string Id { get; set; }
        public string Nome { get; set; }

        public Secretaria()
        {
            Id = "0";
        }
    }
}
