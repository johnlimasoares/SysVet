using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entidades.Cadastro
{
    public class Raca
    {

 
        [Key] 
        public int Id { get; set; }

        [StringLength(50,ErrorMessage = "A Descrição deve ser entre 2 e 50 caracteres!")]
        public string Descricao { get; set; }

        public int EspecieId { get; set; }

        public virtual Especie Especie { get; set; }

        public virtual ICollection<Animal> Animais { get;set; }
    }
}
