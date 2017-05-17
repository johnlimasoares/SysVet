using System.Data.Entity;
using Domain.Entidades.Cadastro.Financeiro;
using Repository.Repositories.Base;

namespace Repository.Repositories {
    public class FinanceiroCentroDeCustoGrupoRepository : Repository<FinanceiroCentroDeCustoGrupo> {
        public void Salvar(FinanceiroCentroDeCustoGrupo financeiroCentroDeCustoGrupo){
            using (ctx){
                ctx.FinaceiroCentroDeCustoGrupos.Add(financeiroCentroDeCustoGrupo);
                ctx.SaveChanges();
            }
        }

        public void Editar(FinanceiroCentroDeCustoGrupo financeiroCentroDeCustoGrupo){
            using (ctx){
                ctx.Entry(financeiroCentroDeCustoGrupo).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }
    }
}