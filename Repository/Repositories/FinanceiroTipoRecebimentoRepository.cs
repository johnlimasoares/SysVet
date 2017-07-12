using Domain.Entidades.Operacao.Financeiro;
using Domain.Entidades.Operacao.Generic;
using Repository.Context;
using Repository.Repositories.Base;

namespace Repository.Repositories {
    public class FinanceiroTipoRecebimentoRepository : Repository<FinanceiroTipoRecebimento> {
        public static void SalvarTipoRecebimentoFinanceiro(BancoContexto ctx, Operacao operacao, FinanceiroTipoRecebimento financeiroTipoRecebimento) {
            financeiroTipoRecebimento.Operacao = operacao;
            financeiroTipoRecebimento.HoraEmissao = financeiroTipoRecebimento.DataEmissao.TimeOfDay;
            ctx.FinanceiroTipoRecebimentos.Add(financeiroTipoRecebimento);
        }
    }
}
