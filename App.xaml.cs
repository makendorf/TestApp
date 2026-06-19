using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using TestApp.DI;
using TestApp.ViewModels;

namespace TestApp
{
    /// <summary>
    /// Класс приложения WPF (App.xaml.cs).
    /// Точка входа: при запуске создаёт главное окно и устанавливает DataContext.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Обработчик события запуска приложения.
        /// Создаёт MainViewModel из контейнера依赖注入 и отображает главное окно.
        /// </summary>
        /// <param name="e">Аргументы запуска.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Получение MainViewModel из DI-контейнера
            var mainViewModel = AppServiceProvider.ServiceProvider.GetRequiredService<MainViewModel>();

            // Создание главного окна и привязка DataContext
            var mainWindow = new MainWindow { DataContext = mainViewModel };
            mainWindow.Show();
        }
    }
}
