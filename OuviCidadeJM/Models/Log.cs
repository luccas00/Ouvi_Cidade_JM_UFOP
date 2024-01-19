using System.ComponentModel.DataAnnotations;

namespace OuviCidadeJM.Models
{
    public class Log
    {
        public string? Resumo;
        public string? Texto;
        public Usuario? Proprietario;
        [Key]
        public string Id;

    }
}
