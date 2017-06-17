using System.Collections.Generic;
using System.Linq;
using Domain.Entidades.Operacao.Financeiro;
using Repository.Repositories;
using Utils;

namespace Business.Financeiro.ContasReceber {
    public static class DuplicataParcelasBusiness {
        public static List<FinanceiroContasReceberParcelas> GerarDemostrativoParcelas(FinanceiroTipoRecebimento financeiroTipoRecebimento) {
            var financeiroContasReceberParcelasList = new List<FinanceiroContasReceberParcelas>();
            var planoDePagamento = new FinanceiroPlanoDePagamentoRepository().GetPlanoPagamento(financeiroTipoRecebimento.FinanceiroPlanoDePagamentoId);

            for (var parcela = 1; parcela <= financeiroTipoRecebimento.QuantidadeParcelas; parcela++) {

                var financeiroContasReceberParcela = new FinanceiroContasReceberParcelas();
                financeiroContasReceberParcela.DataEmissao = financeiroTipoRecebimento.DataEmissao;
                financeiroContasReceberParcela.DataVencimento = financeiroContasReceberParcela.DataEmissao.AddDays(planoDePagamento.IntervaloDias * parcela);
                financeiroContasReceberParcela.Parcela = parcela;
                financeiroContasReceberParcela.ValorTotalBruto = financeiroTipoRecebimento.ValorTotal / financeiroTipoRecebimento.QuantidadeParcelas;
                financeiroContasReceberParcela.NumeroDocumento = string.Format("{0}{1}", parcela, financeiroTipoRecebimento.ClienteId);
                financeiroContasReceberParcelasList.Add(financeiroContasReceberParcela);

                if (IsUltimaParcela(parcela, financeiroTipoRecebimento.QuantidadeParcelas)) {
                    CalcularSobra(financeiroContasReceberParcelasList, financeiroTipoRecebimento.ValorTotal.Round());
                }
            }

            return financeiroContasReceberParcelasList;
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
    }
}
