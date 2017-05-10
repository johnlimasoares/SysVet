using System.Data.Entity;
using Domain.Entidades.Cadastro.Financeiro;
using Repository.Repositories.Base;

namespace Repository.Repositories {
    public class FinanceiroCentroDeCustoRepository : Repository<FinanceiroCentroDeCusto> {
        public void Salvar(FinanceiroCentroDeCusto financeiroCentroDeCusto, int financeiroCentroDeCustoGrupoId) {
            var financeiroCentroDeCustoGrupo = new FinanceiroCentroDeCustoGrupo() { Id = financeiroCentroDeCustoGrupoId };
            using (ctx) {
                ctx.Entry(financeiroCentroDeCustoGrupo).State = EntityState.Unchanged;
                financeiroCentroDeCusto.FinanceiroCentroDeCustoGrupo = financeiroCentroDeCustoGrupo;
                ctx.FinanceiroCentroDeCustos.Add(financeiroCentroDeCusto);
                ctx.SaveChanges();
            }

        }


        public void Editar(FinanceiroCentroDeCusto financeiroCentroDeCusto, int financeiroCentroDeCustoGrupoId) {
            using (ctx) {
                financeiroCentroDeCusto.FinanceiroCentroDeCustoGrupoId = financeiroCentroDeCustoGrupoId;
                ctx.Entry(financeiroCentroDeCusto).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }
    }
}