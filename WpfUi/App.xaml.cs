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

            BooksStore booksStore = new();

            MainWindow = new MainWindow()
            {
                DataContext = new MainWindowViewModel(serviceProvider.GetRequiredService<IBookDataGetter>(),
				booksStore, serviceProvider.GetRequiredService<IFoldersFileNamePairs>(),
				serviceProvider.GetRequiredService<IBooksDataRefresher>())
            };

            MainWindow.Show();
        }

		private static IServiceProvider CreateServiceProvider()
		{
			IServiceCollection services = new ServiceCollection();

			services.AddScoped<IFoldersFileNamePairs, FoldersFileNamePairs>();
			services.AddTransient<ITagsCreator, TagsCreator>();
			services.AddTransient<IBookDataGetter, BookDataGetter>();
			services.AddTransient<IBooksDataRefresher, BooksDataRefresher>();

			return services.BuildServiceProvider();
		}
	}
}

