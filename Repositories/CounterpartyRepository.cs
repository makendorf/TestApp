using System.Collections.Generic;
using NHibernate;
using NHibernate.Transform;
using TestApp.Models;
using TestApp.Services;

namespace TestApp.Repositories
{
    /// <summary>
    /// Интерфейс репозитория для работы с контрагентами.
    /// Наследует общие CRUD-операции от IRepository&lt;Counterparty&gt;.
    /// </summary>
    public interface ICounterpartyRepository : IRepository<Counterparty> { }

    /// <summary>
    /// Репозиторий для работы с контрагентами.
    /// Переопределяет GetAll с жадной загрузкой куратора (JoinAlias)
    /// и дедупликацией через DistinctRootEntity.
    /// </summary>
    public class CounterpartyRepository : Repository<Counterparty>, ICounterpartyRepository
    {
        public CounterpartyRepository(ISessionFactoryProvider sessionProvider) : base(sessionProvider) { }

        /// <summary>
        /// Получить всех контрагентов вместе с данными куратора.
        /// JoinAlias предотвращает N+1 запросы, DistinctRootEntity убирает дубликаты.
        /// </summary>
        public override IEnumerable<Counterparty> GetAll()
        {
            using var session = SessionProvider.OpenSession();
            Counterparty alias = null!;
            return session.QueryOver<Counterparty>(() => alias)
                .JoinAlias(() => alias.Curator, () => alias!.Curator)
                .TransformUsing(Transformers.DistinctRootEntity)
                .List<Counterparty>();
        }
    }
}
