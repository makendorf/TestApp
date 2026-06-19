using System.Collections.Generic;
using NHibernate;
using NHibernate.Transform;
using TestApp.Models;
using TestApp.Services;

namespace TestApp.Repositories
{
    public interface ICounterpartyRepository : IRepository<Counterparty> { }

    public class CounterpartyRepository : Repository<Counterparty>, ICounterpartyRepository
    {
        public CounterpartyRepository(ISessionFactoryProvider sessionProvider) : base(sessionProvider) { }

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
