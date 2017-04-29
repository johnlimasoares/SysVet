using System.ComponentModel.DataAnnotations;
using Domain.Entidades.Operacao;

namespace Domain.Entidades.Cadastro
{
    public class Servico
    {
        [Key]
        public int ID { get; set; }

        public int TipoServicoID { get; set; }
       
        public virtual TipoServico TipoServico { get; set; }

        [MaxLength(200,ErrorMessage = "Máximo de 200 caractéres!")]
        public string Observacao { get; set; }
        public int AtendimentoID { get; set; }
        public virtual Atendimento Atendimento { get; set; }
    }
}