using System;
using Domain.Entidades.Operacao.Financeiro;
using Domain.Entidades.Operacao.Generic;
using Domain.Enum;
using Repository.Context;
using Repository.Repositories.Base;

namespace Repository.Repositories {
    public class FinanceiroMovimentacoesRepository : Repository<FinanceiroMovimentacoes> {
        public static void GerarMovimentacao(BancoContexto ctx, Operacao operacao, TipoMovimentacao tipoMovimentacao, double financeiroCentroDeCustoId, decimal valor, string observacao) {

            var movimentacaoFinanceira = new FinanceiroMovimentacoes();
            movimentacaoFinanceira.TipoMovimentacao = tipoMovimentacao;
            movimentacaoFinanceira.FinanceiroCentroDeCusto = ctx.FinanceiroCentroDeCustos.Find(financeiroCentroDeCustoId);
            movimentacaoFinanceira.Operacao = operacao;
            movimentacaoFinanceira.DataHora = DateTime.Now;
            movimentacaoFinanceira.Observacao = observacao;
            if (tipoMovimentacao == TipoMovimentacao.Credito) {
                movimentacaoFinanceira.Credito = valor;
            } else {
                movimentacaoFinanceira.Debito = valor;
            }

            ctx.FinanceiroMovimentacoes.Add(movimentacaoFinanceira);
        }

    }
}
