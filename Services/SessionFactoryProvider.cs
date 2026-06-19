using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using TestApp.Config;
using TestApp.Mappings;

namespace TestApp.Services
{
    /// <summary>
    /// Интерфейс-провайдер для получения фабрики сессий NHibernate и открытия сессий.
    /// </summary>
    public interface ISessionFactoryProvider
    {
        /// <summary>
        /// Фабрика сессий NHibernate.
        /// </summary>
        ISessionFactory SessionFactory { get; }

        /// <summary>
        /// Открыть новую сессию NHibernate для работы с БД.
        /// </summary>
        ISession OpenSession();
    }

    /// <summary>
    /// Реализация провайдера фабрики сессий.
    /// Конфигурирует NHibernate через FluentNHibernate:
    /// подключение к MySQL, загрузка маппингов из текущей сборки,
    /// автоматический экспорт схемы БД (SchemaExport).
    /// </summary>
    public class SessionFactoryProvider : ISessionFactoryProvider
    {
        /// <summary>
        /// Экземпляр фабрики сессий, созданный при инициализации.
        /// </summary>
        private readonly ISessionFactory _sessionFactory;

        /// <summary>
        /// Создать провайдер, настроив подключение и маппинги NHibernate.
        /// При создании автоматически экспортирует схему БД.
        /// </summary>
        /// <param name="config">Конфигурация базы данных (строка подключения).</param>
        public SessionFactoryProvider(DatabaseConfig config)
        {
            _sessionFactory = Fluently.Configure()
                .Database(MySQLConfiguration.Standard.ConnectionString(config.ConnectionString))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<EmployeeMap>())
                .ExposeConfiguration(cfg => new SchemaExport(cfg).Create(false, true))
                .BuildSessionFactory();
        }

        /// <inheritdoc/>
        public ISessionFactory SessionFactory => _sessionFactory;

        /// <inheritdoc/>
        public ISession OpenSession() => _sessionFactory.OpenSession();
    }
}
