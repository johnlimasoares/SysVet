using System.ComponentModel.DataAnnotations;

namespace Domain.Entidades.Cadastro.Contato
{
    public class Telefone
    {
        [Key]
        public int ID { get; set; }
        
        public string Numero { get; set; }

        public virtual int ClienteID { get; set; }
        public virtual Cliente Cliente { get; set; }

        public virtual int TipoTelefoneId { get; set; }
        public virtual TipoTelefone TipoTelefone { get; set; }
    }
}
