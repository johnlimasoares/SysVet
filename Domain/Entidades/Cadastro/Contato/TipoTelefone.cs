using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entidades.Cadastro.Contato
{
    public class TipoTelefone
    {
        [Key]
        public int Id { get; set; }

        public string Descricao { get; set; }

        public virtual ICollection<Telefone> Animais { get; set; }
    }
}
