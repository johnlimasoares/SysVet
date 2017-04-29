using System;
using System.Linq;
using System.Data.Entity;
using Repository.Context;


namespace Repository.Repositories.Base
{
    public abstract class Repository<TEntity> : IDisposable,
       IRepository<TEntity> where TEntity : class
    {

        public BancoContexto ctx = new BancoContexto();
        public virtual IQueryable<TEntity> GetAll()
        {
            return ctx.Set<TEntity>();
        }

        public IQueryable<TEntity> Get(Func<TEntity, bool> predicate)
        {
            return GetAll().Where(predicate).AsQueryable();
        }

        public TEntity Find(params object[] key)
        {
            return ctx.Set<TEntity>().Find(key);
        }

        public void Atualizar(TEntity obj)
        {
            ctx.Entry(obj).State = EntityState.Modified;
            
        }

        public void SalvarTodos()
        {
            ctx.SaveChanges();
        }

        public void Adicionar(TEntity obj)
        {
            
            ctx.Set<TEntity>().Add(obj);
            
        }

        public void Excluir(Func<TEntity, bool> predicate)
        {
            ctx.Set<TEntity>()
                .Where(predicate).ToList()
                .ForEach(del => ctx.Set<TEntity>().Remove(del));
        }

        public void Dispose()
        {
            ctx.Dispose();
        }
    }
}
