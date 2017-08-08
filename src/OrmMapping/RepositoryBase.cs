using System;
using System.Linq;
using NHibernate;
using NHibernate.Linq;

namespace OrmMapping
{
    class RepositoryBase<TEntity>
    {
        protected readonly ISession session;

        public RepositoryBase(ISession session)
        {
            this.session = session;
        }

        public virtual void Create(TEntity entity)
        {
            WrapTransaction(() =>
            {
                session.Save(entity);
            });
        }

        public virtual TEntity FindById(long id)
        {
            return session.Get<TEntity>(id);
        }

        public virtual IQueryable<TEntity> All()
        {
            return session.Query<TEntity>();
        }

        public virtual void Update(TEntity entity)
        {
            WrapTransaction(() =>
            {
                session.Update(entity);
            });
        }

        public virtual void Delete(TEntity entity)
        {
            WrapTransaction(() =>
            {
                session.Delete(entity);
            });
        }
      
        void WrapTransaction(Action action)
        {
            if (session.Transaction.IsActive)
            {
                action();
            }
            else
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    action();
                    transaction.Commit();
                }
            }
        }
    }
}