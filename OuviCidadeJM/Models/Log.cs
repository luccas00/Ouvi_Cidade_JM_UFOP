using System.ComponentModel.DataAnnotations;

namespace OuviCidadeJM.Models
{
    public class Log
    {
        public string? Resumo { get; set; }
        public string? Texto { get; set; }
        public Usuario? Proprietario { get; set; }
        [Key]
        public string Id { get; set; }

    }
}
