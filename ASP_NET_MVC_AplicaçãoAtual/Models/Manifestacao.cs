using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OuviCidadeV3.Models
{
    public class Manifestacao
    {
        public Cidadao Proprietario { get; set; }
        [Key]
        public string Protocolo { get; set; }
        public string Titulo { get; set; }
        public string Texto { get; set; }
        public DateTime DataCriacao { get; set; }
        public Secretaria? Secretaria { get; set; }
        [ForeignKey("Protocolo")]
        public Resposta? Resposta { get; set; }
        public DateTime? DataResposta { get; set; }

    }
}
