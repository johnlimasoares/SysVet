using System;
using Domain.Enum;

namespace SisVetWeb.Models
{
    public class FinanceiroMovimentacaoViewModel
    {
        public int FinanceiroCentroDeCustoId { get; set; }
        public TipoMovimentacao TipoMovimentacao { get; set; }
        public DateTime DataLancamento { get; set; }
        public decimal Valor { get; set; }
        public string Observacao { get; set; }
    }
}