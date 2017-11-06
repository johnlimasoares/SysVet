using System;
using System.ComponentModel.DataAnnotations;
using Domain.Enum;

namespace Domain.Entidades.Operacao.Financeiro {
    public class FinanceiroContasReceberParcelas {
        [Key]
        public Int64 Id { get; set; }

        public Int64 FinanceiroTipoRecebimentoId { get; set; }
        public FinanceiroTipoRecebimento FinanceiroTipoRecebimento { get; set; }

        public String NumeroDocumento { get; set; }
        public int Parcela { get; set; }
        public Decimal ValorTotalBruto { get; set; }
        public Decimal ValorTotalLiquido { get; set; }
        public Decimal? ValorTotalDesconto { get; set; }
        public Decimal? PercentualDesconto { get; set; }
        public Decimal? ValorTotalJuros { get; set; }
        public Decimal? PercentualJuros { get; set; }
        public Decimal? ValorLiquidado { get; set; }
        public SituacaoParcelaFinanceira SituacaoParcelaFinanceira { get; set; }
        public DateTime DataEmissao { get; set; }
        public TimeSpan HoraEmissao { get; set; }
        public DateTime DataVencimento { get; set; }
        public TimeSpan HoraVencimento { get; set; }
        public DateTime? DataRecebimento { get; set; }
        public TimeSpan? HoraRecebimento { get; set; }
        public DateTime? DataCancelamento { get; set; }
        public TimeSpan? HoraCancelamento { get; set; }

        [StringLength(100, ErrorMessage = "A Descrição deve ser entre 2 e 100 caracteres!")]
        public String Observacoes { get; set; }
    }
}