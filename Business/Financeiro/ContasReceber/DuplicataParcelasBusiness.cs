using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Domain.Entidades.Cadastro.Financeiro;
using Domain.Entidades.Operacao.Financeiro;
using Domain.Entidades.Operacao.Generic;
using Domain.Enum;
using Repository.Context;
using Repository.Repositories;
using Utils;

namespace Business.Financeiro.ContasReceber
{
    public static class DuplicataParcelasBusiness
    {
        public static List<FinanceiroContasReceberParcelas> GerarDemostrativoParcelas(FinanceiroTipoRecebimento financeiroTipoRecebimento)
        {
            var financeiroContasReceberParcelasList = new List<FinanceiroContasReceberParcelas>();

            var planoDePagamento = new FinanceiroPlanoDePagamentoRepository().GetPlanoPagamento(financeiroTipoRecebimento.FinanceiroPlanoDePagamentoId);

            for (var parcela = 1; parcela <= financeiroTipoRecebimento.QuantidadeParcelas; parcela++)
            {

                var financeiroContasReceberParcela = new FinanceiroContasReceberParcelas();
                financeiroContasReceberParcela.DataEmissao = financeiroTipoRecebimento.DataEmissao;
                financeiroContasReceberParcela.HoraEmissao = DateTime.Now.TimeOfDay;
                financeiroContasReceberParcela.DataVencimento = financeiroContasReceberParcela.DataEmissao.AddDays(planoDePagamento.IntervaloDias * parcela);
                financeiroContasReceberParcela.Parcela = parcela;
                financeiroContasReceberParcela.ValorTotalBruto = financeiroTipoRecebimento.ValorTotal / financeiroTipoRecebimento.QuantidadeParcelas;
                financeiroContasReceberParcela.ValorTotalLiquido = financeiroContasReceberParcela.ValorTotalBruto;
                financeiroContasReceberParcela.NumeroDocumento = string.Format("{0}{1}{2}-{3}", financeiroTipoRecebimento.ClienteId, planoDePagamento.Id, GetDataParaNumeroDocumento(), parcela);
                financeiroContasReceberParcela.FinanceiroTipoRecebimento = financeiroTipoRecebimento;
                financeiroContasReceberParcela.VerificarSeUtilizouPlanoAvista(planoDePagamento);
                financeiroContasReceberParcelasList.Add(financeiroContasReceberParcela);

                if (IsUltimaParcela(parcela, financeiroTipoRecebimento.QuantidadeParcelas))
                {
                    CalcularSobra(financeiroContasReceberParcelasList, financeiroTipoRecebimento.ValorTotal.Round());
                }
            }

            return financeiroContasReceberParcelasList;
        }

        public static void VerificarSeUtilizouPlanoAvista(this FinanceiroContasReceberParcelas financeiroContasReceberParcela, FinanceiroPlanoDePagamento planoDePagamento)
        {
            if (planoDePagamento.IntervaloDias == 0 && planoDePagamento.QuantidadeParcelas == 1)
            {
                financeiroContasReceberParcela.SituacaoParcelaFinanceira = SituacaoParcelaFinanceira.Liquidado;
                financeiroContasReceberParcela.DataRecebimento = DateTime.Now;
                financeiroContasReceberParcela.ValorLiquidado = financeiroContasReceberParcela.ValorTotalLiquido;
            }
            else
            {
                financeiroContasReceberParcela.SituacaoParcelaFinanceira = SituacaoParcelaFinanceira.Aberto;
            }
        }

        private static void CalcularSobra(List<FinanceiroContasReceberParcelas> financeiroContasReceberParcelasList, decimal valorTotal)
        {
            var valorTotalParcelas = financeiroContasReceberParcelasList.Sum(x => x.ValorTotalBruto.Round());
            var valorSobra = valorTotal - valorTotalParcelas;
            if (valorSobra != 0)
            {
                financeiroContasReceberParcelasList[UltimaParcela(financeiroContasReceberParcelasList)].ValorTotalBruto += valorSobra;               
            }
        }

        private static int UltimaParcela(List<FinanceiroContasReceberParcelas> financeiroContasReceberParcelasList)
        {
            return financeiroContasReceberParcelasList.Count - 1;
        }

        private static bool IsUltimaParcela(int parcelaAtual, int quantidadeParcelas)
        {
            return parcelaAtual == quantidadeParcelas;
        }

        private static string GetDataParaNumeroDocumento()
        {
            var dataAtual = DateTime.Now;
            return dataAtual.DayOfYear.ToString() + dataAtual.Hour + dataAtual.Minute + dataAtual.Second;
        }

        public static void SalvarRegistroFinanceiro(List<FinanceiroContasReceberParcelas> financeiroContasReceberParcelasList, FinanceiroTipoRecebimento financeiroTipoRecebimento)
        {
            using (var ctx = new BancoContexto())
            {
                var operacao = OperacaoRepository.GerarOperacao(ctx);
                FinanceiroTipoRecebimentoRepository.SalvarTipoRecebimentoFinanceiro(ctx, operacao, financeiroTipoRecebimento);
                FinanceiroContasReceberParcelasRepository.SalvarParcelasGeradas(ctx, operacao, financeiroContasReceberParcelasList, financeiroTipoRecebimento.FinanceiroCentroDeCustoId);
                ctx.SaveChanges();
            }
        }
    }
}
