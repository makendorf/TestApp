using System.Collections.Generic;
using NHibernate;
using TestApp.Services;

namespace TestApp.Repositories
{
    /// <summary>
    /// Интерфейс通用ного репозитория для работы с сущностями.
    /// Определяет базовые CRUD-операции: получение по Id, получение всех,
    /// сохранение и удаление.
    /// </summary>
    /// <typeparam name="T">Тип сущности.</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Получить сущность по идентификатору.
        /// </summary>
        T? GetById(int id);

        /// <summary>
        /// Получить все сущности.
        /// </summary>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Сохранить или обновить сущность в базе данных.
        /// </summary>
        void Save(T entity);

        /// <summary>
        /// Удалить сущность из базы данных.
        /// </summary>
        void Delete(T entity);
    }

    /// <summary>
    /// Универсальный репозиторий — базовая реализация CRUD для любой сущности.
    /// Использует NHibernate сессии через ISessionFactoryProvider.
    /// </summary>
    /// <typeparam name="T">Тип сущности.</typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        /// <summary>
        /// Провайдер сессий NHibernate.
        /// </summary>
        protected readonly ISessionFactoryProvider SessionProvider;

        /// <summary>
        /// Создать экземпляр репозитория.
        /// </summary>
        /// <param name="sessionProvider">Провайдер фабрики сессий.</param>
        public Repository(ISessionFactoryProvider sessionProvider)
        {
            SessionProvider = sessionProvider;
        }

        /// <summary>
        /// Получить сущность по идентификатору.
        /// </summary>
        public virtual T? GetById(int id)
        {
            using var session = SessionProvider.OpenSession();
            return session.Get<T>(id);
        }

        /// <summary>
        /// Получить все сущности указанного типа.
        /// </summary>
        public virtual IEnumerable<T> GetAll()
        {
            using var session = SessionProvider.OpenSession();
            return session.QueryOver<T>().List<T>();
        }

        /// <summary>
        /// Сохранить или обновить сущность.
        /// Выполняет SaveOrUpdate внутри транзакции.
        /// </summary>
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
                // Откат транзакции при любой ошибке
                transaction.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Удалить сущность из базы данных.
        /// Выполняет Delete внутри транзакции.
        /// </summary>
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
                // Откат транзакции при любой ошибке
                transaction.Rollback();
                throw;
            }
        }
    }
}
