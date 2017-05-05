using System.ComponentModel.DataAnnotations;

namespace Domain.Entidades.Operacao.Financeiro
{
    public class FinanceiroCentroDeCustoGrupo
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "A Descrição deve ser entre 2 e 20 caracteres!")]
        public string Descricao { get; set; }

    }
}