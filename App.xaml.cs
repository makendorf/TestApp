using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using TestApp.DI;
using TestApp.ViewModels;

namespace TestApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainViewModel = AppServiceProvider.ServiceProvider.GetRequiredService<MainViewModel>();
            var mainWindow = new MainWindow { DataContext = mainViewModel };
            mainWindow.Show();
        }
    }
}
