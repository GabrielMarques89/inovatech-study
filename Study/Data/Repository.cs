using System;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using Study.Data.Models;

namespace Study.Data
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly ISession session;

        protected IQueryable<T> set { get { return session.Query<T>(); } }

        public Repository(ISession session)
        {
            this.session = session;
        }

        public virtual IQueryable<T> Queryable()
        {
            return set;
        }

        public virtual T FindById(long id)
        {
            return session.Get<T>(id);
        }

        public virtual void Save(T entidade)
        {
            session.SaveOrUpdate(entidade);
        }

        public virtual void Merge(T entidade)
        {
            session.Merge(entidade);
        }

        public virtual void Delete(long id)
        {
            session.Delete(FindById(id));
        }

        public virtual void Delete(T entidade)
        {
            session.Delete(entidade);
        }

        public virtual void Flush()
        {
            session.Flush();
        }

        public virtual void Clear()
        {
            session.Clear();
        }

    }

    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> Queryable();

        T FindById(long id);

        void Save(T entity);

        void Merge(T entity);

        void Delete(long id);

        void Delete(T entity);

        void Flush();

        void Clear();
    }
}