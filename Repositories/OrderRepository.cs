using System.Collections.Generic;
using NHibernate;
using NHibernate.Transform;
using TestApp.Models;
using TestApp.Services;

namespace TestApp.Repositories
{
    public interface IOrderRepository : IRepository<Order> { }

    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(ISessionFactoryProvider sessionProvider) : base(sessionProvider) { }

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
