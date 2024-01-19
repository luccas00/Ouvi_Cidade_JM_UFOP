using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OuviCidadeJM.Models
{
    public class Contato
    {
        public string? Nome { get; set; }
        public string? Telefone { get; set; }

        [Key]
        public string CPF { get; set; }
        public DateTime? DataNascimento { get; set; }
        public int? Idade { get; set; }
        public bool? Ativo { get; set; }

    }
}
