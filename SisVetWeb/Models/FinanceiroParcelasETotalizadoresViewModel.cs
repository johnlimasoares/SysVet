using System;
using System.Collections.Generic;
using System.Linq;
using Domain.EntidadesLeitura.Operacao.Financeiro;
using Domain.Enum;

namespace SisVetWeb.Models
{
    public class FinanceiroParcelasETotalizadoresViewModel
    {
        public List<FinanceiroContasReceberParcelasDapper> FinanceiroContasReceberParcelasDapperList { get; set; }
        public decimal TotalVencidas { get; private set; }
        public decimal TotalAVencer { get; private set; }
        public decimal TotalVenceHoje { get; private set; }
        public decimal TotalRecebidas { get; private set; }
        public decimal TotalEmitidas { get; private set; }
        public decimal TotalAReceber { get; set; }

        public FinanceiroParcelasETotalizadoresViewModel PreencherTotalizadores()
        {
            var dataAtual = DateTime.Now;
            this.TotalVencidas = FinanceiroContasReceberParcelasDapperList.Where(y => y.DataVencimento < dataAtual && y.SituacaoParcelaFinanceira == SituacaoParcelaFinanceira.Aberto).Sum(x => x.ValorTotalLiquido);
            this.TotalAVencer = FinanceiroContasReceberParcelasDapperList.Where(y => y.DataVencimento > dataAtual && y.SituacaoParcelaFinanceira == SituacaoParcelaFinanceira.Aberto).Sum(x => x.ValorTotalLiquido);
            this.TotalVenceHoje = FinanceiroContasReceberParcelasDapperList.Where(y => y.DataVencimento == dataAtual && y.SituacaoParcelaFinanceira == SituacaoParcelaFinanceira.Aberto).Sum(x => x.ValorTotalLiquido);
            this.TotalRecebidas = FinanceiroContasReceberParcelasDapperList.Where(y => y.SituacaoParcelaFinanceira == SituacaoParcelaFinanceira.Liquidado).Sum(x => x.ValorLiquidado);
            var parcelaDefault = FinanceiroContasReceberParcelasDapperList.FirstOrDefault();
            this.TotalEmitidas = parcelaDefault != null ? parcelaDefault.ValorTotalEmitidas : 0;
            this.TotalAReceber = TotalAVencer + TotalVenceHoje + TotalVencidas;
            return this;
        }
    }

}