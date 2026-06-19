using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestApp.Config;
using TestApp.Repositories;
using TestApp.Services;
using TestApp.ViewModels;

namespace TestApp.DI
{
    public static class AppServiceProvider
    {
        private static ServiceProvider? _serviceProvider;

        public static ServiceProvider ServiceProvider =>
            _serviceProvider ??= BuildServiceProvider();

        private static ServiceProvider BuildServiceProvider()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var databaseConfig = new DatabaseConfig();
            configuration.GetSection("Database").Bind(databaseConfig);

            var services = new ServiceCollection();

            services.AddSingleton(databaseConfig);
            services.AddSingleton<ISessionFactoryProvider, SessionFactoryProvider>();

            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<ICounterpartyRepository, CounterpartyRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddTransient<EmployeeViewModel>();
            services.AddTransient<CounterpartyViewModel>();
            services.AddTransient<OrderViewModel>();
            services.AddTransient<MainViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
