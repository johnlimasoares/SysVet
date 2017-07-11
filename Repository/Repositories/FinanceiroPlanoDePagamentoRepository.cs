using System;
using System.Data.Entity;
using System.Linq;
using Domain.Entidades.Cadastro.Financeiro;
using Repository.Repositories.Base;

namespace Repository.Repositories {
    public class FinanceiroPlanoDePagamentoRepository : Repository<FinanceiroPlanoDePagamento> {

        public void Salvar(FinanceiroPlanoDePagamento financeiroPlanoDePagamento) {
            using (ctx) {
                ctx.FinanceiroPlanosDePagamentos.Add(financeiroPlanoDePagamento);
                ctx.SaveChanges();
            }
        }


        public void Editar(FinanceiroPlanoDePagamento financeiroPlanoDePagamento) {
            using (ctx) {
                ctx.Entry(financeiroPlanoDePagamento).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }


        public FinanceiroPlanoDePagamento Excluir(int id) {
            var planoDePagamento = this.Find(id);
            if (planoDePagamento != null) {
                this.Excluir(c => c == planoDePagamento);
                this.SalvarTodos();
                return planoDePagamento;
            }

            throw new Exception("Plano inexistente no banco de dados");
        }

        public FinanceiroPlanoDePagamento GetPlanoPagamento(long financeiroPlanoDePagamentoId) {
            return Find(financeiroPlanoDePagamentoId);
        }

        public string GetDescricaoPlano(double planoPagamentoId) {
            var planoPagamento = this.GetAll()
                      .Where(x => x.Id == planoPagamentoId)
                      .Select(p => new { p.Descricao })
                      .FirstOrDefault();

            return planoPagamento == null ? string.Empty : planoPagamento.Descricao;
        }
    }
}