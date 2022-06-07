using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Library;

namespace DataAccess.Library.SqliteDataAccess
{
	public class QueryData<T> : IQueryData<T>
	{
		private readonly ISqliteConnector _sqliteConnector;

		public QueryData(ISqliteConnector sqliteConnector)
		{
			_sqliteConnector = sqliteConnector;
		}
		public List<T> GetList(string sql)
		{
			using IDbConnection cnn = new SQLiteConnection(_sqliteConnector.LoadConnectionString());

			List<T> list = cnn.Query<T>(sql, new DynamicParameters()).ToList();

			return list;
		}

		public T GetFirst(string sql)
		{
			using IDbConnection cnn = new SQLiteConnection(_sqliteConnector.LoadConnectionString());

			T model = cnn.Query<T>(sql, new DynamicParameters()).First();

			return model;
		}
	}
}
