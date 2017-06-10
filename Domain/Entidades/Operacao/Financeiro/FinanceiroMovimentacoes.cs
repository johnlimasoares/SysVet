using System;
using Domain.Entidades.Cadastro.Financeiro;
using Domain.Enum;

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
    }
}