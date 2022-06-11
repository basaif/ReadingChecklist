using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfUi.ViewModels;

namespace WpfUi.HostBuilders
{
	public static class AddViewsHostBuilderExtensions
	{
		public static IHostBuilder AddViews(this IHostBuilder host)
		{
			host.ConfigureServices((services) =>
			{
				services.AddScoped(s => new MainWindow(s.GetRequiredService<MainWindowViewModel>()));
			});

			return host;
		}
	}
}
