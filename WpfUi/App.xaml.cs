using Microsoft.Extensions.DependencyInjection;
using WpfUi.Stores;
using WpfUi.ViewModels;
using System;
using System.Windows;
using DomainLogic.Library;
using FileSystemUtilities.Library;

namespace WpfUi
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
			IServiceProvider serviceProvider = CreateServiceProvider();

			BookDataGetter bookDataGetter = new();
            BooksStore booksStore = new();
            TagsCreator tagsCreator = new();
            BookDataGenerator bookDataGenerator = new(serviceProvider.GetRequiredService<IFoldersFileNamePairs>(), tagsCreator);
            BooksDataRefresher booksDataRefresher = new(serviceProvider.GetRequiredService<IFoldersFileNamePairs>(), tagsCreator);

            MainWindow = new MainWindow()
            {
                DataContext = new MainWindowViewModel(bookDataGetter, booksStore, serviceProvider.GetRequiredService<IFoldersFileNamePairs>(), bookDataGenerator, booksDataRefresher)
            };

            MainWindow.Show();
        }

		private static IServiceProvider CreateServiceProvider()
		{
			IServiceCollection services = new ServiceCollection();

			services.AddScoped<IFoldersFileNamePairs, FoldersFileNamePairs>();

			return services.BuildServiceProvider();
		}
	}
}

