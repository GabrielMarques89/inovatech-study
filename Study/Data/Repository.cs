using System;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using Study.Models;

namespace Study.Data
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly ISession Session;

        protected IQueryable<T> Set { get { return Session.Query<T>(); } }

        public Repository(ISession session)
        {
            this.Session = session;
        }

        public virtual IQueryable<T> Queryable()
        {
            return Set;
        }

        public virtual T FindById(long id)
        {
            return Session.Get<T>(id);
        }

        public virtual void Save(T entidade)
        {
            Session.SaveOrUpdate(entidade);
        }

        public virtual void Merge(T entidade)
        {
            Session.Merge(entidade);
        }

        public virtual void Delete(long id)
        {
            Session.Delete(FindById(id));
        }

        public virtual void Delete(T entidade)
        {
            Session.Delete(entidade);
        }

        public virtual void Flush()
        {
            Session.Flush();
        }

        public virtual void Clear()
        {
            Session.Clear();
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