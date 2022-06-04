using FileManagementLibrary;
using Microsoft.Extensions.DependencyInjection;
using ReadingChecklistLogicLibrary;
using ReadingChecklistWpf.Stores;
using ReadingChecklistWpf.ViewModels;
using System;
using System.Security.Principal;
using System.Windows;

namespace ReadingChecklistWpf
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

