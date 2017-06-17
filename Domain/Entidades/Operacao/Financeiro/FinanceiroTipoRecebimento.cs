using System;
using System.ComponentModel.DataAnnotations;
using Domain.Entidades.Cadastro;
using Domain.Entidades.Cadastro.Financeiro;
using Domain.Enum;

namespace Domain.Entidades.Operacao.Financeiro {
    public class FinanceiroTipoRecebimento {
        [Key]
        public Int64 Id { get; set; }

        public Int64 OperacaoId { get; set; }
        public Generic.Operacao Operacao { get; set; }

        public Int64 ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        public Int64 FinanceiroCentroDeCustoId { get; set; }
        public FinanceiroCentroDeCusto FinanceiroCentroDeCusto { get; set; }

        public Int64 FinanceiroPlanoDePagamentoId { get; set; }
        public FinanceiroPlanoDePagamento FinanceiroPlanoDePagamento { get; set; }

        public TipoOperacaoFinanceira TipoOperacaoFinanceira { get; set; }

        public int QuantidadeParcelas { get; set; }

        public Decimal ValorTotal { get; set; }

        public Decimal ValorPorParcela { get; set; }

        public DateTime DataEmissao { get; set; }

        public TimeSpan HoraEmissao { get; set; }

        public DateTime DataVencimento { get; set; }

        public TimeSpan HoraVencimento { get; set; }

        public DateTime? DataLiquidacao { get; set; }

        public TimeSpan? HoraLiquidacao { get; set; }

        public String Referencia { get; set; }

        public String Observacao { get; set; }

    }
}