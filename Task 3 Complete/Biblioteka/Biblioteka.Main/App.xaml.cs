using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Threading;
using Biblioteka.Data;
using Biblioteka.Interfaces;
using Biblioteka.Services;
using Biblioteka.ViewModels;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Biblioteka.Main
{
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }
        public IConfiguration Configuration { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            // Utwórz kontekst bazy danych SQLite
            DatabaseFacade facade = new DatabaseFacade(new BibliotekaContext());
            facade.EnsureCreated();

            ServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            MainWindow mainWindow = ServiceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(ServiceCollection serviceCollection)
        {
            // Skonfiguruj SQLite jako dostawcę bazy danych
            serviceCollection.AddDbContext<BibliotekaContext>();

            // Dodaj pozostałe usługi
            serviceCollection.AddSingleton<IDialogService, DialogService>();
            serviceCollection.AddSingleton<MainWindowViewModel>();
            serviceCollection.AddSingleton<MainWindow>();
        }

        private void ApplicationDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("An unhandled exception just occurred: " + e.Exception.InnerException, "Exception", MessageBoxButton.OK, MessageBoxImage.Warning);
            e.Handled = true;
        }
    }
}
