using Domain.Entidades.Operacao.Generic;
using Repository.Context;
using Repository.Repositories.Base;
namespace Repository.Repositories
{
    public class OperacaoRepository : Repository<Operacao>
    {
        public static Operacao GerarOperacao(BancoContexto ctx){
            return ctx.Operacoes.Add(new Operacao().GerarOperacao());
        }
    }
}