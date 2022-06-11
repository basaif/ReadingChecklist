using DomainLogic.Library;
using DomainLogic.Library.Creators;
using DomainLogic.Library.Services;
using FileSystemUtilities.Library;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUi.HostBuilders
{
	public static class AddDomainClassesHostBuildersExtensions
	{
		public static IHostBuilder AddDomainClasses(this IHostBuilder host)
		{
			host.ConfigureServices((services) =>
			{
				services.AddScoped<IFoldersFileNamePairs, FoldersFileNamePairs>();
				services.AddTransient<ITagsCreator, TagsCreator>();
				services.AddTransient<IBookTagStructureCreator, BookTagStructureCreator>();

				services.AddTransient<IBookDataService, BookDataService>();
				services.AddTransient<ITagDataService, TagDataService>();
				services.AddTransient<IBookUpdater, BookUpdater>();

			});

			return host;
		}
	}
}
