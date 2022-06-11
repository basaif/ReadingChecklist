using DataAccess.Library.ModelDataServices;
using DataAccess.Library.SqliteDataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUi.HostBuilders
{
	public static class AddDataClassesHostBuilderExtensions
	{
		public static IHostBuilder AddDataClasses(this IHostBuilder host)
		{
			host.ConfigureServices((context, services) =>
			{
				string connectionString = context.Configuration.GetConnectionString("Default");

				services.AddSingleton<ISqliteConnector>(new SqliteConnector(connectionString));

				services.AddTransient<ISaveData, SaveData>();
				services.AddTransient<IQueryData<BookModel>, QueryData<BookModel>>();
				services.AddTransient<IQueryData<TagModel>, QueryData<TagModel>>();

				services.AddTransient<ISqliteBookData, SqliteBookData>();
				services.AddTransient<ISqliteTagData, SqliteTagData>();

			});

			return host;
		}
	}
}
