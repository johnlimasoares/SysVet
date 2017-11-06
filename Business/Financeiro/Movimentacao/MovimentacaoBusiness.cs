using System;
using System.Globalization;
using Domain.Entidades.Operacao.Financeiro;
using Domain.Entidades.Operacao.Generic;
using Domain.Enum;
using Repository.Context;
using Repository.Repositories;

namespace Business.Financeiro.Movimentacao
{
    public static class MovimentacaoBusiness
    {
        public static void GerarMovimentacaoCreditoOriundasDeContasReceber(BancoContexto ctx, Operacao operacao, decimal valorParcela, int numeroParcela, int quantidadeParcela, double financeiroCentroDeCustoId, string nomeCliente)
        {
            var observacao = string.Format("Crédito originado de baixa de contas a receber. Parcela '{0} de {1}' - Data:'{2}' - Cliente:'{3}'", numeroParcela, quantidadeParcela, DateTime.Now.ToString("G", new CultureInfo("pt-BR")), nomeCliente);
            FinanceiroMovimentacoesRepository.GerarMovimentacao(ctx, operacao, TipoMovimentacao.Credito, financeiroCentroDeCustoId, valorParcela, observacao);
        }

        public static void GerarMovimentacaoDebitoOriundasDeContasReceber(BancoContexto ctx, Operacao operacao, decimal valorParcela, int numeroParcela, int quantidadeParcela, double financeiroCentroDeCustoId, string nomeCliente, bool isCancelamentoParcela)
        {
            var observacao = string.Format("Débito originado de cancelamento de '{0}' de contas a receber. Parcela '{1} de {2}' - Data:'{3}' - Cliente:'{4}'", isCancelamentoParcela ? "PARCELA" : "BAIXA", numeroParcela, quantidadeParcela, DateTime.Now.ToString("G", new CultureInfo("pt-BR")), nomeCliente);
            FinanceiroMovimentacoesRepository.GerarMovimentacao(ctx, operacao, TipoMovimentacao.Debito, financeiroCentroDeCustoId, valorParcela, observacao);
        }

    }
}