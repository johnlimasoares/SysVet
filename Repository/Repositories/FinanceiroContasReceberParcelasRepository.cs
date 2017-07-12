using System.Collections.Generic;
using Domain.Entidades.Operacao.Financeiro;
using Domain.Entidades.Operacao.Generic;
using Domain.Enum;
using Repository.Context;
using Repository.Repositories.Base;

namespace Repository.Repositories {
    public class FinanceiroContasReceberParcelasRepository : Repository<FinanceiroContasReceberParcelas> {
        public static void SalvarParcelasGeradas(BancoContexto ctx, Operacao operacao, List<FinanceiroContasReceberParcelas> financeiroContasReceberParcelasList, double financeiroCentroDeCustoId) {
            foreach (var parcela in financeiroContasReceberParcelasList) {
                ctx.FinanceiroContasReceberParcelas.Add(parcela);
                if (parcela.SituacaoParcelaFinanceira == SituacaoParcelaFinanceira.Liquidado) {
                    FinanceiroMovimentacoesRepository.GerarMovimentacaoEntrada(ctx, operacao, TipoMovimentacao.Credito, financeiroCentroDeCustoId, parcela.ValorTotalLiquido);
                }
            }
        }
    }
}