using System;
using Domain.Enum;

namespace Domain.EntidadesLeitura.Operacao.Financeiro
{
    public class FinanceiroMovimentacoesDapper
    {
        public int Id { get; set; }
        public string CentroCusto { get; set; }
        public TipoMovimentacao TipoMovimentacao { get; set; }
        public string TipoMovimentacaoDescricao { get; set; }
        public DateTime DataHora { get; set; }
        public decimal Valor { get; set; }
        public string  Observacao { get; set; }
    }
}
