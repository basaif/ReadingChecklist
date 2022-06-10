using Microsoft.Extensions.DependencyInjection;
using WpfUi.Stores;
using WpfUi.ViewModels;
using System;
using System.Windows;
using FileSystemUtilities.Library;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using DomainLogic.Library.Services;
using DomainLogic.Library.Creators;
using System.IO;
using DataAccess.Library.SqliteDataAccess;
using Models.Library;
using DataAccess.Library.ModelDataServices;
using DomainLogic.Library;

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
			_host = Host.CreateDefaultBuilder()
				.ConfigureAppConfiguration(c =>
				{
					c.SetBasePath(Directory.GetCurrentDirectory());
					c.AddJsonFile("appsettings.json");
#if DEBUG
					c.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
#else
c.AddJsonFile("appsettings.Production.json", optional: true, reloadOnChange: true);
#endif
				})
				.ConfigureServices((context, services) =>
				{
					string connectionString = context.Configuration.GetConnectionString("Default");

					services.AddSingleton<ISqliteConnector>(new SqliteConnector(connectionString));

					services.AddTransient<ISaveData, SaveData>();
					services.AddTransient<IQueryData<BookModel>, QueryData<BookModel>>();
					services.AddTransient<IQueryData<TagModel>, QueryData<TagModel>>();

					services.AddTransient<ISqliteBookData, SqliteBookData>();
					services.AddTransient<ISqliteTagData, SqliteTagData>();

					services.AddScoped<IFoldersFileNamePairs, FoldersFileNamePairs>();
					services.AddTransient<ITagsCreator, TagsCreator>();
					services.AddTransient<IBookTagStructureCreator, BookTagStructureCreator>();

					services.AddTransient<IBookDataService, BookDataService>();
					services.AddTransient<ITagDataService, TagDataService>();
					services.AddTransient<IBooksUpdater, BooksUpdater>();

					services.AddSingleton<BookStore>();

					services.AddScoped<MainWindowViewModel>();
					services.AddScoped(services =>
					{
						HomeViewModel homeViewModel = new(
							services.GetRequiredService<IBookDataService>(),
							services.GetRequiredService<BookStore>(),
							services.GetRequiredService<IFoldersFileNamePairs>(),
							services.GetRequiredService<IBookTagStructureCreator>(),
							services.GetRequiredService<IBooksUpdater>());

						HomeViewModel.LoadViewModel(services.GetRequiredService<IBookDataService>(),
							services.GetRequiredService<BookStore>(),
							services.GetRequiredService<IFoldersFileNamePairs>(),
							services.GetRequiredService<IBookTagStructureCreator>(),
							services.GetRequiredService<IBooksUpdater>());

						return homeViewModel;
					});
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

