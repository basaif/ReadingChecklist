using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUi.HostBuilders
{
	public static class AddConfigurationHostBuilderExtensions
	{
		public static IHostBuilder AddConfiguration(this IHostBuilder host)
		{
			host.ConfigureAppConfiguration(c =>
			{
				c.SetBasePath(Directory.GetCurrentDirectory());
				c.AddJsonFile("appsettings.json");
#if DEBUG
				c.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
#else
c.AddJsonFile("appsettings.Production.json", optional: true, reloadOnChange: true);
#endif
			});

			return host;
		}
	}
}
