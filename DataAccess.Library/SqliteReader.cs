using Dapper;
using System.Data.SQLite;
using System.Data;
using Models.Library;

namespace DataAccess.Library
{
	public static class SqliteReader
	{
		public static TagModel ReadTagById(int id)
		{
			using IDbConnection cnn = new SQLiteConnection(SqliteConnector.LoadConnectionString());

			var sql = $"select id, TagName from Tag where id = {id}";

			var output = cnn.Query<TagModel>(sql, new DynamicParameters()).First();

			return output;
		}

		public static List<TagModel> ReadAllTags()
		{
			using IDbConnection cnn = new SQLiteConnection(SqliteConnector.LoadConnectionString());

			var sql = "select id, TagName from Tag";
			var output = cnn.Query<TagModel>(sql, new DynamicParameters()).ToList();

			return output;
		}

		public static List<TagModel> ReadTagsByBook(int bookId)
		{
			using IDbConnection cnn = new SQLiteConnection(SqliteConnector.LoadConnectionString());

			var sql = $"select * from Tag where Tag.Id In (select TagId from Book_Tag where BookId = {bookId})";

			var output = cnn.Query<TagModel>(sql, new DynamicParameters()).ToList();

			return output;
		}

		public static BookModel ReadBookById(int id)
		{
			using IDbConnection cnn = new SQLiteConnection(SqliteConnector.LoadConnectionString());

			var sql = $"select * from Book where id = {id}";

			var output = cnn.Query<BookModel>(sql, new DynamicParameters()).First();

			output.Tags = ReadTagsByBook(id);

			return output;
		}

		public static List<BookModel> ReadAllBooks()
		{
			using IDbConnection cnn = new SQLiteConnection(SqliteConnector.LoadConnectionString());

			var sql = "select * from Book";
			var output = cnn.Query<BookModel>(sql, new DynamicParameters()).ToList();

			Parallel.ForEach(output, (book) => book.Tags = ReadTagsByBook(book.Id));

			return output;
		}

		public static List<BookModel> ReadBooksByTag(int tagId)
		{
			using IDbConnection cnn = new SQLiteConnection(SqliteConnector.LoadConnectionString());

			var sql = $"select * from Book where Book.Id In (select BookId from Book_Tag where TagId = {tagId})";

			var output = cnn.Query<BookModel>(sql, new DynamicParameters()).ToList();

			Parallel.ForEach(output, (book) => book.Tags = ReadTagsByBook(book.Id));

			return output;
		}
	}
}
