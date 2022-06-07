using Dapper;

namespace DataAccess.Library.SqliteDataAccess
{
	public interface ISaveData
	{
		void ExecuteId(string sql, int id);
		void ExecuteModel<T>(string sql, T model);
		void ExecuteParameters(string sql, DynamicParameters p);
		int ExecuteParametersReturnId(string sql, DynamicParameters p);
	}
}