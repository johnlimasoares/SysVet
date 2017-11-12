using System;
using System.Globalization;
using Domain.Entidades.Operacao.Financeiro;
using Domain.Entidades.Operacao.Generic;
using Domain.Enum;
using Repository.Context;

namespace Business.Financeiro.Movimentacao
{
    public static class MovimentacaoBusiness
    {
        public static void GerarMovimentacaoCreditoOriundasDeContasReceber(BancoContexto ctx, Operacao operacao, OrigemMovimentacao origem, decimal valorParcela, int numeroParcela, int quantidadeParcela, double financeiroCentroDeCustoId, string nomeCliente, string observacoes)
        {
            var observacao = string.Format("Crédito originado de baixa de contas a receber. |Cliente: {0}  |Data:'{1}'   |Parcela: {2} de {3}   |Observações de Baixa: {4}'", nomeCliente, DateTime.Now.ToString("G", new CultureInfo("pt-BR")), numeroParcela, quantidadeParcela, observacoes);
            GerarMovimentacao(ctx, operacao, TipoMovimentacao.Credito, origem, financeiroCentroDeCustoId, valorParcela, observacao);
        }

        public static void GerarMovimentacaoDebitoOriundasDeContasReceber(BancoContexto ctx, Operacao operacao, OrigemMovimentacao origem, decimal valorParcela, int numeroParcela, int quantidadeParcela, double financeiroCentroDeCustoId, string nomeCliente, bool isCancelamentoParcela)
        {
            var observacao = string.Format("Débito originado de cancelamento de '{0}' de contas a receber.   |Parcela '{1} de {2}'   |Data:'{3}'   |Cliente:'{4}'", isCancelamentoParcela ? "PARCELA" : "BAIXA", numeroParcela, quantidadeParcela, DateTime.Now.ToString("G", new CultureInfo("pt-BR")), nomeCliente);
            GerarMovimentacao(ctx, operacao, TipoMovimentacao.Debito, origem, financeiroCentroDeCustoId, valorParcela, observacao);
        }

        public static void GerarMovimentacaoManual(FinanceiroMovimentacoes movimentacao)
        {
            using (var ctx = new BancoContexto())
            {
                var tipoMovimento = movimentacao.TipoMovimentacao == TipoMovimentacao.Credito ? "Crédito" : "Débito";
                var valor = movimentacao.TipoMovimentacao == TipoMovimentacao.Credito ? movimentacao.Credito : movimentacao.Debito;
                var operacao = new Operacao();
                operacao.Data = DateTime.Now;
                ctx.Operacoes.Add(operacao);

                var observacao = string.Format("{0} lançado manualmente.   |Data geração: {1}   |Observações: {2}", tipoMovimento, DateTime.Now.ToString("G", new CultureInfo("pt-BR")), movimentacao.Observacao);
                GerarMovimentacao(ctx, operacao, movimentacao.TipoMovimentacao, movimentacao.OrigemMovimentacao, movimentacao.FinanceiroCentroDeCustoId, valor, observacao);
                ctx.SaveChanges();
            }
        }

        private static void GerarMovimentacao(BancoContexto ctx, Operacao operacao, TipoMovimentacao tipoMovimentacao, OrigemMovimentacao origem, double financeiroCentroDeCustoId, decimal valor, string observacao)
        {
            var movimentacaoFinanceira = new FinanceiroMovimentacoes();
            movimentacaoFinanceira.TipoMovimentacao = tipoMovimentacao;
            movimentacaoFinanceira.FinanceiroCentroDeCusto = ctx.FinanceiroCentroDeCustos.Find(financeiroCentroDeCustoId);
            movimentacaoFinanceira.Operacao = operacao;
            movimentacaoFinanceira.DataHora = DateTime.Now;
            movimentacaoFinanceira.Observacao = observacao;
            movimentacaoFinanceira.OrigemMovimentacao = origem;
            if (tipoMovimentacao == TipoMovimentacao.Credito)
            {
                movimentacaoFinanceira.Credito = valor;
            }
            else
            {
                movimentacaoFinanceira.Debito = valor;
            }

            ctx.FinanceiroMovimentacoes.Add(movimentacaoFinanceira);
        }


    }
}