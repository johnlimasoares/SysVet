using System.Collections.Generic;
using Domain.Entidades.Operacao.Financeiro;
using Repository.Context;
using Repository.Repositories.Base;

namespace Repository.Repositories {
    public class FinanceiroContasReceberParcelasRepository : Repository<FinanceiroContasReceberParcelas> {
        public static void SalvarParcelasGeradas(BancoContexto ctx, List<FinanceiroContasReceberParcelas> financeiroContasReceberParcelasList){
            foreach (var financeiroContasReceberParcela in financeiroContasReceberParcelasList) {
                ctx.FinanceiroContasReceberParcelas.Add(financeiroContasReceberParcela);
            }
        }
    }
}