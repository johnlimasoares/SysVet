using System.Collections.Generic;
using Domain.Entidades.Operacao.Financeiro;
using Repository.Context;
using Repository.Repositories.Base;

namespace Repository.Repositories {
    public class FinanceiroTipoRecebimentoRepository : Repository<FinanceiroTipoRecebimento> {
        public static void SalvarRegistroFinanceiro(BancoContexto ctx, FinanceiroTipoRecebimento financeiroTipoRecebimento) {
            var operacao = new Domain.Entidades.Operacao.Generic.Operacao().GerarOperacao();
            ctx.Operacoes.Add(operacao);

            financeiroTipoRecebimento.Operacao = operacao;
            financeiroTipoRecebimento.HoraEmissao = financeiroTipoRecebimento.DataEmissao.TimeOfDay;
            ctx.FinanceiroTipoRecebimentos.Add(financeiroTipoRecebimento);

          
        }
    }
}
