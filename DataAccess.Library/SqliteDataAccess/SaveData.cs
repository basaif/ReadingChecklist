using System.Data.SQLite;
using System.Data;
using Dapper;
using Models.Library;

namespace DataAccess.Library.SqliteDataAccess
{
	public class SaveData : ISaveData
	{
		private readonly ISqliteConnector _sqliteConnector;

		public SaveData(ISqliteConnector sqliteConnector)
		{
			_sqliteConnector = sqliteConnector;
		}
		public void ExecuteParameters(string sql, DynamicParameters p)
		{
			using IDbConnection cnn = new SQLiteConnection(_sqliteConnector.LoadConnectionString());

			cnn.Execute(sql, p);
		}

		public int ExecuteParametersReturnId(string sql, DynamicParameters p)
		{
			using IDbConnection cnn = new SQLiteConnection(_sqliteConnector.LoadConnectionString());

			long id = cnn.ExecuteScalar<long>(sql, p);
			return (int)id;
		}


		public void ExecuteModel<T>(string sql, T model)
		{
			using IDbConnection cnn = new SQLiteConnection(_sqliteConnector.LoadConnectionString());

			cnn.Execute(sql, model);
		}

		public void ExecuteId(string sql, int id)
		{
			using IDbConnection cnn = new SQLiteConnection(_sqliteConnector.LoadConnectionString());

			cnn.Execute(sql, id);
		}
	}
}
