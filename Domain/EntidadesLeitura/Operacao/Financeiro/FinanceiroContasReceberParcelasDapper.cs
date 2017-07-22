using System;

namespace Domain.EntidadesLeitura.Operacao.Financeiro {
    public class FinanceiroContasReceberParcelasDapper {
        
        public int ClienteId { get; set; }
        public string ClienteNome { get; set; }
        public int Parcela{ get; set; }
        public int TotalParcelas{ get; set; }
        public DateTime DataEmissao { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime DataRecebimento{ get; set; }
        public DateTime DataCancelamento{ get; set; }
        public decimal ValorTotalLiquido { get; set; }
        public decimal ValorLiquidado { get; set; }
    }
}
