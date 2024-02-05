using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OuviCidadeV3.Models
{
    public class Cidadao
    {
        [Key]
        public string CPF { get; set; }
        public string Nome { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string SecretKey { get; set; }
        public string Endereco { get; set; }
        public DateTime DataNascimento { get; set; }
        public bool Ativo { get; set; }
        
        public Cidadao()
        {
            this.CPF = "SYSTEM";
            this.Nome = "SYSTEM";
        }

        public Cidadao(string CPF)
        {
            this.CPF = CPF;
        }

    }

}
