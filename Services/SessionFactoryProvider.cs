using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using TestApp.Config;
using TestApp.Mappings;

namespace TestApp.Services
{
    public interface ISessionFactoryProvider
    {
        ISessionFactory SessionFactory { get; }
        ISession OpenSession();
    }

    public class SessionFactoryProvider : ISessionFactoryProvider
    {
        private readonly ISessionFactory _sessionFactory;

        public SessionFactoryProvider(DatabaseConfig config)
        {
            _sessionFactory = Fluently.Configure()
                .Database(MySQLConfiguration.Standard.ConnectionString(config.ConnectionString))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<EmployeeMap>())
                .ExposeConfiguration(cfg => new SchemaExport(cfg).Create(false, true))
                .BuildSessionFactory();
        }

        public ISessionFactory SessionFactory => _sessionFactory;

        public ISession OpenSession() => _sessionFactory.OpenSession();
    }
}
