using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestApp.Config;
using TestApp.Repositories;
using TestApp.Services;
using TestApp.ViewModels;

namespace TestApp.DI
{
    /// <summary>
    /// Статический класс-поставщик сервисов для контейнера依赖注入.
    /// Настраивает Microsoft.Extensions.DependencyInjection:
    /// конфигурацию, репозитории, ViewModel.
    /// </summary>
    public static class AppServiceProvider
    {
        /// <summary>
        /// Экземпляр ServiceProvider (ленивая инициализация).
        /// </summary>
        private static ServiceProvider? _serviceProvider;

        /// <summary>
        /// Получить экземпляр ServiceProvider.
        /// Создаётся при первом обращении.
        /// </summary>
        public static ServiceProvider ServiceProvider =>
            _serviceProvider ??= BuildServiceProvider();

        /// <summary>
        /// Создать и настроить ServiceProvider со всеми регистрациями сервисов.
        /// Читает конфигурацию из appsettings.json, регистрирует:
        /// DatabaseConfig (Singleton), SessionFactoryProvider (Singleton),
        /// репозитории (Scoped), ViewModel (Transient).
        /// </summary>
        private static ServiceProvider BuildServiceProvider()
        {
            // Загрузка конфигурации из appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Привязка секции «Database» к объекту DatabaseConfig
            var databaseConfig = new DatabaseConfig();
            configuration.GetSection("Database").Bind(databaseConfig);

            var services = new ServiceCollection();

            // Регистрация конфигурации и провайдера сессий как Singleton (один на всё приложение)
            services.AddSingleton(databaseConfig);
            services.AddSingleton<ISessionFactoryProvider, SessionFactoryProvider>();

            // Регистрация репозиториев как Scoped (одна сессия на запрос)
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<ICounterpartyRepository, CounterpartyRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            // Регистрация ViewModel как Transient (новый экземпляр при каждом запросе)
            services.AddTransient<EmployeeViewModel>();
            services.AddTransient<CounterpartyViewModel>();
            services.AddTransient<OrderViewModel>();
            services.AddTransient<MainViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
