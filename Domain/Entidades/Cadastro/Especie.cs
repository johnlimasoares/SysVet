using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entidades.Cadastro
{
    public class Especie {
        [Key]
        public int ID { get; set; }

        [StringLength(50, ErrorMessage = "O tamanho máximo são 50 caracteres!")]
        public string Descricao { get; set; }

        public virtual ICollection<Raca> Racas { get; set; }
    }
}
