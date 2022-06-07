using System.Data.SQLite;

namespace DataAccess.Library.SqliteDataAccess
{
	public class SqliteConnector : ISqliteConnector
	{
		private readonly string _connectionString;

		public SqliteConnector(string connectionString)
		{
			_connectionString = connectionString;
		}
		public string LoadConnectionString()
		{
			return new SQLiteConnection(_connectionString).ConnectionString;
		}
	}
}