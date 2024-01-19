using System.ComponentModel.DataAnnotations;

namespace OuviCidadeJM.Models
{
    public class Manifestacao
    {
        public string? Proprietario { get; set; }
        public string? Protocolo { get; set; }
        public string? Detalhes { get; set; }
        public string? Resumo { get; set; }
        public DateTime? DataCriacao { get; set; }
        [Key]
        public string ID { get; set; }

    }
}
