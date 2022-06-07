namespace DataAccess.Library.SqliteDataAccess
{
	public interface IQueryData<T>
	{
		T GetFirst(string sql);
		List<T> GetList(string sql);
	}
}