using System;
using Domain.Entidades.Cadastro.Financeiro;
using Domain.Enum;
using Domain.Entidades.Operacao.Generic;

namespace Domain.Entidades.Operacao.Financeiro {
    public class FinanceiroMovimentacoes {
        public Int64 Id { get; set; }

        public Int64 FinanceiroCentroDeCustoId { get; set; }
        public FinanceiroCentroDeCusto FinanceiroCentroDeCusto { get; set; }

        public Int64 OperacaoId { get; set; }
        public Generic.Operacao Operacao { get; set; }


        public TipoMovimentacao TipoMovimentacaoEnum { get; set; }

        public DateTime DataHora { get; set; }

        public Decimal Credito { get; set; }

        public Decimal Debito { get; set; }

        public Decimal Saldo { get; set; }

        public FinanceiroMovimentacoes GerarMovimentacaoDebito(FinanceiroCentroDeCusto financeiroCentroDeCusto, Generic.Operacao operacao, DateTime dataHora, decimal debito) {
            this.FinanceiroCentroDeCusto = financeiroCentroDeCusto;
            this.Operacao = operacao;
            this.TipoMovimentacaoEnum = TipoMovimentacao.Debito;
            this.DataHora = dataHora;
            this.Debito = debito;
            return this;
        }

        public FinanceiroMovimentacoes GerarMovimentacaoCredito(FinanceiroCentroDeCusto financeiroCentroDeCusto, Generic.Operacao operacao, DateTime dataHora, decimal credito) {
            this.FinanceiroCentroDeCusto = financeiroCentroDeCusto;
            this.Operacao = operacao;
            this.TipoMovimentacaoEnum = TipoMovimentacao.Credito;
            this.DataHora = dataHora;
            this.Credito = credito;
            return this;
        }
    }
}