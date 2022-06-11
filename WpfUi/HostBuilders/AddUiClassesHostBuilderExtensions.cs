using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfUi.Stores;

namespace WpfUi.HostBuilders
{
	public static class AddUiClassesHostBuilderExtensions
	{
		public static IHostBuilder AddUiClasses(this IHostBuilder host)
		{
			host.ConfigureServices((services) =>
			{
				services.AddSingleton<BookStore>();
			});

			return host;
		}
	}
}
