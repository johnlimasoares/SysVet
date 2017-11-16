using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using Business.Cliente;
using Business.Financeiro.Movimentacao;
using Domain.Entidades.Cadastro.Financeiro;
using Domain.Entidades.Operacao.Financeiro;
using Domain.Entidades.Operacao.Generic;
using Domain.Enum;
using Repository.Context;
using Repository.Repositories;
using Utils;

namespace Business.Financeiro.ContasReceber
{
    public static class ParcelasBusiness
    {
        public static List<FinanceiroContasReceberParcelas> GerarDemonstrativoParcelas(FinanceiroTipoRecebimento financeiroTipoRecebimento)
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
                financeiroContasReceberParcela.ValorTotalBruto = (financeiroTipoRecebimento.ValorTotal / financeiroTipoRecebimento.QuantidadeParcelas).Round();
                financeiroContasReceberParcela.ValorTotalLiquido = financeiroContasReceberParcela.ValorTotalBruto;
                financeiroContasReceberParcela.NumeroDocumento = string.Format("{0}{1}{2}-{3}", financeiroTipoRecebimento.ClienteId, planoDePagamento.Id, GetDataParaNumeroDocumento(), parcela);
                financeiroContasReceberParcela.FinanceiroTipoRecebimento = financeiroTipoRecebimento;
                financeiroContasReceberParcela.VerificarSeUtilizouPlanoAvista(planoDePagamento);
                financeiroContasReceberParcelasList.Add(financeiroContasReceberParcela);

                if (IsUltimaParcela(parcela, financeiroTipoRecebimento.QuantidadeParcelas))
                {
                    CalcularDiferenca(financeiroContasReceberParcelasList, financeiroTipoRecebimento.ValorTotal.Round());
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
                financeiroContasReceberParcela.HoraRecebimento = DateTime.Now.TimeOfDay;
                financeiroContasReceberParcela.ValorLiquidado = financeiroContasReceberParcela.ValorTotalLiquido;
            }
            else
            {
                financeiroContasReceberParcela.SituacaoParcelaFinanceira = SituacaoParcelaFinanceira.Aberto;
            }
        }

        private static void CalcularDiferenca(List<FinanceiroContasReceberParcelas> financeiroContasReceberParcelasList, decimal valorTotal)
        {
            var valorTotalParcelas = financeiroContasReceberParcelasList.Sum(x => x.ValorTotalBruto.Round());
            var valorSobra = valorTotal - valorTotalParcelas;
            if (valorSobra != 0)
            {
                financeiroContasReceberParcelasList[UltimaParcela(financeiroContasReceberParcelasList)].ValorTotalBruto += valorSobra;
                financeiroContasReceberParcelasList[UltimaParcela(financeiroContasReceberParcelasList)].ValorTotalLiquido += valorSobra;
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

        public static void SalvarParcelasGeradas(List<FinanceiroContasReceberParcelas> financeiroContasReceberParcelasList, FinanceiroTipoRecebimento financeiroTipoRecebimento)
        {
            using (var ctx = new BancoContexto())
            {
                var operacao = new Operacao();
                operacao.Data = DateTime.Now;
                ctx.Operacoes.Add(operacao);

                SalvarTipoRecebimentoFinanceiro(ctx, operacao, financeiroTipoRecebimento);
                SalvarParcelasGeradas(ctx, operacao, financeiroContasReceberParcelasList, financeiroTipoRecebimento);
                ctx.SaveChanges();
            }
        }

        public static void SalvarTipoRecebimentoFinanceiro(BancoContexto ctx, Operacao operacao, FinanceiroTipoRecebimento financeiroTipoRecebimento)
        {
            financeiroTipoRecebimento.Operacao = operacao;
            financeiroTipoRecebimento.HoraEmissao = financeiroTipoRecebimento.DataEmissao.TimeOfDay;
            ctx.FinanceiroTipoRecebimentos.Add(financeiroTipoRecebimento);
        }

        public static void SalvarParcelasGeradas(BancoContexto ctx, Operacao operacao, List<FinanceiroContasReceberParcelas> financeiroContasReceberParcelasList, FinanceiroTipoRecebimento financeiroTipoRecebimento)
        {
            foreach (var parcela in financeiroContasReceberParcelasList)
            {
                ctx.FinanceiroContasReceberParcelas.Add(parcela);
                if (parcela.SituacaoParcelaFinanceira == SituacaoParcelaFinanceira.Liquidado)
                {
                    var nomeCliente = ClienteBusiness.GetNomeCliente(ctx, financeiroTipoRecebimento.ClienteId);
                    MovimentacaoBusiness.GerarMovimentacaoCreditoOriundasDeContasReceber(ctx, operacao, OrigemMovimentacao.ContasReceber, parcela.ValorTotalLiquido, parcela.Parcela, parcela.FinanceiroTipoRecebimento.QuantidadeParcelas, financeiroTipoRecebimento.FinanceiroCentroDeCustoId, nomeCliente, financeiroTipoRecebimento.Observacao);
                }
            }
        }

        public static void BaixarParcela(FinanceiroContasReceberParcelas parcelaRecebida)
        {
            using (var ctx = new BancoContexto())
            {
                var parcela = ctx.FinanceiroContasReceberParcelas.Find(parcelaRecebida.Id);
                parcela.ValorLiquidado = parcela.ValorTotalLiquido;
                parcela.DataRecebimento = parcelaRecebida.DataRecebimento;
                parcela.SituacaoParcelaFinanceira = SituacaoParcelaFinanceira.Liquidado;
                parcela.HoraRecebimento = parcelaRecebida.HoraRecebimento;
                parcela.Observacoes = parcelaRecebida.Observacoes;
                ctx.Entry(parcela).State = EntityState.Modified;

                var tipoRecebimento = ctx.FinanceiroTipoRecebimentos.Where(x => x.Id == parcela.FinanceiroTipoRecebimentoId).Select(p => new { p.Id, p.Operacao, p.FinanceiroCentroDeCustoId, p.Cliente.Nome, p.QuantidadeParcelas }).First();
                MovimentacaoBusiness.GerarMovimentacaoCreditoOriundasDeContasReceber(ctx, tipoRecebimento.Operacao, OrigemMovimentacao.ContasReceber, parcela.ValorTotalLiquido, parcela.Parcela, tipoRecebimento.QuantidadeParcelas, tipoRecebimento.FinanceiroCentroDeCustoId, tipoRecebimento.Nome, parcela.Observacoes);
                ctx.SaveChanges();
            }

        }

        public static void CancelarBaixa(int parcelaId)
        {
            using (var ctx = new BancoContexto())
            {
                var parcela = ctx.FinanceiroContasReceberParcelas.Find(parcelaId);
                parcela.ValorLiquidado = 0;
                parcela.DataRecebimento = null;
                parcela.SituacaoParcelaFinanceira = SituacaoParcelaFinanceira.Aberto;
                parcela.HoraRecebimento = null;
                parcela.Observacoes = null;
                parcela.DataCancelamento = DateTime.Now;
                parcela.HoraCancelamento = DateTime.Now.TimeOfDay;
                ctx.Entry(parcela).State = EntityState.Modified;

                var tipoRecebimento = ctx.FinanceiroTipoRecebimentos.Where(x => x.Id == parcela.FinanceiroTipoRecebimentoId).Select(p => new { p.Id, p.Operacao, p.FinanceiroCentroDeCustoId, p.Cliente.Nome, p.QuantidadeParcelas }).First();
                MovimentacaoBusiness.GerarMovimentacaoDebitoOriundasDeContasReceber(ctx, tipoRecebimento.Operacao, OrigemMovimentacao.ContasReceber, parcela.ValorTotalLiquido, parcela.Parcela, tipoRecebimento.QuantidadeParcelas, tipoRecebimento.FinanceiroCentroDeCustoId, tipoRecebimento.Nome, false);
                ctx.SaveChanges();
            }
        }

        public static void CancelarParcela(int parcelaId)
        {
            using (var ctx = new BancoContexto())
            {
                var parcela = ctx.FinanceiroContasReceberParcelas.Find(parcelaId);
                parcela.ValorLiquidado = 0;
                parcela.DataRecebimento = null;
                var estavaLiquidada = parcela.SituacaoParcelaFinanceira == SituacaoParcelaFinanceira.Liquidado;
                parcela.SituacaoParcelaFinanceira = SituacaoParcelaFinanceira.Cancelado;
                parcela.HoraRecebimento = null;
                parcela.DataCancelamento = DateTime.Now;
                parcela.HoraCancelamento = DateTime.Now.TimeOfDay;
                ctx.Entry(parcela).State = EntityState.Modified;

                if (estavaLiquidada)
                {
                    var tipoRecebimento = ctx.FinanceiroTipoRecebimentos.Where(x => x.Id == parcela.FinanceiroTipoRecebimentoId).Select(p => new { p.Id, p.Operacao, p.FinanceiroCentroDeCustoId, p.Cliente.Nome, p.QuantidadeParcelas }).First();
                    MovimentacaoBusiness.GerarMovimentacaoDebitoOriundasDeContasReceber(ctx, tipoRecebimento.Operacao, OrigemMovimentacao.ContasReceber, parcela.ValorTotalLiquido, parcela.Parcela, tipoRecebimento.QuantidadeParcelas, tipoRecebimento.FinanceiroCentroDeCustoId, tipoRecebimento.Nome, true);
                }
                ctx.SaveChanges();
            }
        }


    }
}
