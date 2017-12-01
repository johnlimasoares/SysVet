using System;
using Domain.Enum;

namespace Domain.EntidadesLeitura.ReportsModel
{
    public class FinanceiroContasReceberReport
    {
        public int ClienteId { get; set; }
        public string ClienteNome { get; set; }
        public int CentroCustoId { get; set; }
        public string CentroCustoDescricao { get; set; }
        public DateTime DataEmissao { get; set; }
        public DateTime DataRecebimento { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime DataCancelamento { get; set; }
        public decimal ValorTotalLiquido { get; set; }
        public decimal ValorLiquidado { get; set; }
        public SituacaoParcelaFinanceira SituacaoParcelaFinanceira { get; set; }
        
    }
}