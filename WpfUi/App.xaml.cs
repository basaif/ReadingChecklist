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
using System.Threading.Tasks;
using WpfUi.HostBuilders;

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
				.AddConfiguration()
				.AddDataClasses()
				.AddDomainClasses()
				.AddUiClasses()
				.AddViewModels()
				.AddViews()
				.Build();
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

