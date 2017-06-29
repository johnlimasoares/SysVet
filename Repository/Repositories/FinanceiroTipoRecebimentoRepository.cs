using System.Collections.Generic;
using Domain.Entidades.Operacao.Financeiro;
using Repository.Repositories.Base;

namespace Repository.Repositories {
    public class FinanceiroTipoRecebimentoRepository : Repository<FinanceiroTipoRecebimento> {
        public void SalvarRegistroFinanceiro(List<FinanceiroContasReceberParcelas> financeiroContasReceberParcelasList, FinanceiroTipoRecebimento financeiroTipoRecebimento) {
            using (ctx) {
                var operacao = new Domain.Entidades.Operacao.Generic.Operacao().GerarOperacao();
                ctx.Operacoes.Add(operacao);

                financeiroTipoRecebimento.Operacao = operacao;
                financeiroTipoRecebimento.HoraEmissao = financeiroTipoRecebimento.DataEmissao.TimeOfDay;
                ctx.FinanceiroTipoRecebimentos.Add(financeiroTipoRecebimento);

                //foreach (var financeiroContasReceberParcela in financeiroContasReceberParcelasList){
                //    ctx.FinanceiroContasReceberParcelas.Add(financeiroContasReceberParcela);
                //}

                ctx.SaveChanges();
            }
        }
    }
}
