using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entidades.Cadastro.Financeiro;
using Domain.Entidades.Operacao.Financeiro;
using Domain.Enum;
using Repository.Repositories;
using Utils;

namespace Business.Financeiro.ContasReceber {
    public static class DuplicataParcelasBusiness {
        public static List<FinanceiroContasReceberParcelas> GerarDemostrativoParcelas(FinanceiroTipoRecebimento financeiroTipoRecebimento) {
            var financeiroContasReceberParcelasList = new List<FinanceiroContasReceberParcelas>();

            var planoDePagamento = new FinanceiroPlanoDePagamentoRepository().GetPlanoPagamento(financeiroTipoRecebimento.FinanceiroPlanoDePagamento.Id);

            for (var parcela = 1; parcela <= financeiroTipoRecebimento.QuantidadeParcelas; parcela++) {

                var financeiroContasReceberParcela = new FinanceiroContasReceberParcelas();
                financeiroContasReceberParcela.DataEmissao = financeiroTipoRecebimento.DataEmissao;
                financeiroContasReceberParcela.HoraEmissao = financeiroContasReceberParcela.DataEmissao.TimeOfDay;
                financeiroContasReceberParcela.DataVencimento = financeiroContasReceberParcela.DataEmissao.AddDays(planoDePagamento.IntervaloDias * parcela);
                financeiroContasReceberParcela.HoraVencimento = financeiroContasReceberParcela.DataVencimento.TimeOfDay;
                financeiroContasReceberParcela.Parcela = parcela;
                financeiroContasReceberParcela.ValorTotalBruto = financeiroTipoRecebimento.ValorTotal / financeiroTipoRecebimento.QuantidadeParcelas;
                financeiroContasReceberParcela.ValorTotalLiquido = financeiroContasReceberParcela.ValorTotalBruto;
                financeiroContasReceberParcela.NumeroDocumento = string.Format("{0}{1}-{2}", financeiroTipoRecebimento.Cliente.Id, planoDePagamento.Id, parcela);
                financeiroContasReceberParcela.FinanceiroTipoRecebimento = financeiroTipoRecebimento;
                financeiroContasReceberParcela.SituacaoParcelaContasReceberEnum = GetSituacaoContaReceber(planoDePagamento);
                financeiroContasReceberParcelasList.Add(financeiroContasReceberParcela);

                if (IsUltimaParcela(parcela, financeiroTipoRecebimento.QuantidadeParcelas)) {
                    CalcularSobra(financeiroContasReceberParcelasList, financeiroTipoRecebimento.ValorTotal.Round());
                }
            }

            return financeiroContasReceberParcelasList;
        }

        private static SituacaoParcelaContasReceber GetSituacaoContaReceber(FinanceiroPlanoDePagamento planoDePagamento) {
            return planoDePagamento.IntervaloDias == 0 && planoDePagamento.QuantidadeParcelas == 1
                ? SituacaoParcelaContasReceber.Liquidado
                : SituacaoParcelaContasReceber.Aberto;
        }

        private static void CalcularSobra(List<FinanceiroContasReceberParcelas> financeiroContasReceberParcelasList, decimal valorTotal) {
            var valorTotalParcelas = financeiroContasReceberParcelasList.Sum(x => x.ValorTotalBruto.Round());
            var valorSobra = valorTotal - valorTotalParcelas;
            if (valorSobra != 0) {
                financeiroContasReceberParcelasList[UltimaParcela(financeiroContasReceberParcelasList)].ValorTotalBruto += valorSobra;
            }
        }

        private static int UltimaParcela(List<FinanceiroContasReceberParcelas> financeiroContasReceberParcelasList) {
            return financeiroContasReceberParcelasList.Count - 1;
        }

        private static bool IsUltimaParcela(int parcelaAtual, int quantidadeParcelas) {
            return parcelaAtual == quantidadeParcelas;
        }

        public static void ConcluirRegistroFinanceiro(List<FinanceiroContasReceberParcelas> financeiroContasReceberParcelasList, FinanceiroTipoRecebimento financeiroTipoRecebimento) {
            new FinanceiroTipoRecebimentoRepository().SalvarRegistroFinanceiro(financeiroContasReceberParcelasList, financeiroTipoRecebimento);

        }
    }
}
