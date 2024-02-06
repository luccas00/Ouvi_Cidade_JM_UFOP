using System.ComponentModel.DataAnnotations;

namespace OuviCidadeV3.Models
{
    public class Admin 
    {
        [Key]
        public string ID { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string SecretKey { get; set; }
        public string Endereco { get; set; }
        public DateTime DataNascimento { get; set; }
        public Secretaria Secretaria { get; set; }
        public bool Ativo { get; set; }
    }
}
