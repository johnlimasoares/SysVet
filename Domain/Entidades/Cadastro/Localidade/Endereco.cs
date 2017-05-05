using System.ComponentModel.DataAnnotations;

namespace Domain.Entidades.Cadastro.Localidade
{
    public class Endereco {
        
        [Key]
        public int Id { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "A Descrição deve ser entre 2 e 50 caracteres!")]
        public string Logradouro { get; set; }

        public string Cep { get; set; }

        public string Complemento { get; set; }
        public string Numero { get; set; }

        public int? CidadeId { get; set; }
        public virtual Cidade Cidade { get; set; }

        public int ClienteId { get; set; }
        public virtual Cliente Cliente { get; set; }
    }
}
