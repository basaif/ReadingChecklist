using Microsoft.Extensions.DependencyInjection;
using WpfUi.Stores;
using WpfUi.ViewModels;
using System;
using System.Windows;
using DomainLogic.Library;
using FileSystemUtilities.Library;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace WpfUi
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
    {
		private readonly IHost _host;

		public App()
		{
			_host = Host.CreateDefaultBuilder().
				ConfigureServices((services) =>
				{
					services.AddScoped<IFoldersFileNamePairs, FoldersFileNamePairs>();
					services.AddTransient<ITagsCreator, TagsCreator>();
					services.AddTransient<IBookDataGetter, BookDataGetter>();
					services.AddTransient<IBooksDataRefresher, BooksDataRefresher>();

				}).Build();
		}
		private void Application_Startup(object sender, StartupEventArgs e)
        {

            BooksStore booksStore = new();

            MainWindow = new MainWindow()
            {
                DataContext = new MainWindowViewModel(_host.Services.GetRequiredService<IBookDataGetter>(),
				booksStore, _host.Services.GetRequiredService<IFoldersFileNamePairs>(),
				_host.Services.GetRequiredService<IBooksDataRefresher>())
            };

            MainWindow.Show();
        }
	}
}

