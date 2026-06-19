using System.Collections.Generic;
using NHibernate;
using NHibernate.Transform;
using TestApp.Models;
using TestApp.Services;

namespace TestApp.Repositories
{
    /// <summary>
    /// Интерфейс репозитория для работы с заказами.
    /// Наследует общие CRUD-операции от IRepository&lt;Order&gt;.
    /// </summary>
    public interface IOrderRepository : IRepository<Order> { }

    /// <summary>
    /// Репозиторий для работы с заказами.
    /// Переопределяет GetAll с жадной загрузкой сотрудника и контрагента
    /// через JoinAlias и дедупликацией через DistinctRootEntity.
    /// </summary>
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(ISessionFactoryProvider sessionProvider) : base(sessionProvider) { }

        /// <summary>
        /// Получить все заказы вместе с данными сотрудника и контрагента.
        /// JoinAlias предотвращает N+1 запросы, DistinctRootEntity убирает дубликаты.
        /// </summary>
        public override IEnumerable<Order> GetAll()
        {
            using var session = SessionProvider.OpenSession();
            Order alias = null!;
            return session.QueryOver<Order>(() => alias)
                .JoinAlias(() => alias.Employee, () => alias!.Employee)
                .JoinAlias(() => alias.Counterparty, () => alias!.Counterparty)
                .TransformUsing(Transformers.DistinctRootEntity)
                .List<Order>();
        }
    }
}
