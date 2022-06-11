using DomainLogic.Library.Creators;
using FileSystemUtilities.Library;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfUi.Stores;
using WpfUi.ViewModels;

namespace WpfUi.HostBuilders
{
	public static class AddViewModelsHostBuildersExtensions
	{
		public static IHostBuilder AddViewModels(this IHostBuilder host)
		{
			host.ConfigureServices((services) =>
			{
				services.AddScoped<MainWindowViewModel>();
				services.AddScoped(services => CreateHomeViewModel(services));
			});

			return host;
		}

		private static HomeViewModel CreateHomeViewModel(IServiceProvider services)
		{
			HomeViewModel homeViewModel = new(
						services.GetRequiredService<BookStore>(),
						services.GetRequiredService<IFoldersFileNamePairs>(),
						services.GetRequiredService<IBookTagStructureCreator>());

			HomeViewModel.LoadViewModel(
				services.GetRequiredService<BookStore>(),
				services.GetRequiredService<IFoldersFileNamePairs>(),
				services.GetRequiredService<IBookTagStructureCreator>());

			return homeViewModel;
		}
	}
}
