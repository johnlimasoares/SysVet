using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entidades.Cadastro.Localidade
{
    public class Cidade
    {
        
        public int ID { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "A Descrição deve ser entre 2 e 50 caracteres!")]
        public string Descricao { get; set; }

        [StringLength(2,MinimumLength = 2,ErrorMessage = "A sigla deve conter 2 caracteres")]
        public string SiglaUnidadeFederativa { get; set; }

        public virtual ICollection<Endereco> enderecos { get; set; } 

        //public UnidadeFederativa UnidadeFederativa { get; set; }
    }
}
