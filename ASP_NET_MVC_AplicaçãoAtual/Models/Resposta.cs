using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OuviCidadeV3.Models
{
    public class Resposta
    {
        [Key]
        public string Protocolo { get; set; }
        public Manifestacao Manifestacao { get; set; }
        [ForeignKey("ID")]
        public Admin Admin { get; set; }
        public string Texto { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
