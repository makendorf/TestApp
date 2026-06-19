using System.Collections.Generic;
using NHibernate;
using TestApp.Services;

namespace TestApp.Repositories
{
    public interface IRepository<T> where T : class
    {
        T? GetById(int id);
        IEnumerable<T> GetAll();
        void Save(T entity);
        void Delete(T entity);
    }

    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ISessionFactoryProvider SessionProvider;

        public Repository(ISessionFactoryProvider sessionProvider)
        {
            SessionProvider = sessionProvider;
        }

        public virtual T? GetById(int id)
        {
            using var session = SessionProvider.OpenSession();
            return session.Get<T>(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            using var session = SessionProvider.OpenSession();
            return session.QueryOver<T>().List<T>();
        }

        public virtual void Save(T entity)
        {
            using var session = SessionProvider.OpenSession();
            using var transaction = session.BeginTransaction();
            try
            {
                session.SaveOrUpdate(entity);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public virtual void Delete(T entity)
        {
            using var session = SessionProvider.OpenSession();
            using var transaction = session.BeginTransaction();
            try
            {
                session.Delete(entity);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
