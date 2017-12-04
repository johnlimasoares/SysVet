using System;
using Domain.Enum;

namespace Domain.EntidadesLeitura.ReportsModel
{
    public class FinanceiroMovimentacoesReportModel
    {
        public string CentroCustoDescricao { get; set; }
        public DateTime DataHora { get; set; }
        public decimal CreditoDebito { get; set; }
        public TipoMovimentacao TipoMovimentacao { get; set; }
        public string TipoMovimentacaoDescricao { get; set; }
    }
}