using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Entidades.Operacao;

namespace Domain.Entidades.Cadastro
{
    public class Vacina
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "A Descrição deve ser entre 2 e 50 caracteres!")]
        public string Descricao { get; set; }

        public int IntervaloDias { get; set; }

        public int Doses { get; set; }

        public virtual ICollection<Vacinacao> Vacinacoes { get; set; }  
    }
}