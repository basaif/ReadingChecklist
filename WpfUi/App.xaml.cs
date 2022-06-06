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
					services.AddTransient<IBookTagStructureCreator, BookTagStructureCreator>();
					services.AddTransient<IBookDataService, BookDataService>();

					services.AddSingleton<BooksStore>();

					services.AddScoped<MainWindowViewModel>();
					services.AddScoped(s => new MainWindow(s.GetRequiredService<MainWindowViewModel>()));

				}).Build();
		}
		private void Application_Startup(object sender, StartupEventArgs e)
        {
			_host.Start();

            MainWindow = _host.Services.GetRequiredService<MainWindow>();

			MainWindow.Show();
        }

		protected override async void OnExit(ExitEventArgs e)
		{
			await _host.StopAsync();
			_host.Dispose();
			base.OnExit(e);
		}
	}
}

