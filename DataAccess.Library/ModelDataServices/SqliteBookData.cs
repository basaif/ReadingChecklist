using Dapper;
using Models.Library;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Library.SqliteDataAccess;

namespace DataAccess.Library.ModelDataServices
{
	public class SqliteBookData : ISqliteBookData
	{
		private readonly ISaveData _saveData;
		private readonly IQueryData<BookModel> _queryData;
		private readonly ISqliteTagData _sqliteTagData;

		public SqliteBookData(ISaveData saveData, IQueryData<BookModel> queryData, ISqliteTagData sqliteTagData)
		{
			_saveData = saveData;
			_queryData = queryData;
			_sqliteTagData = sqliteTagData;
		}
		public void CreateBook(BookModel book)
		{
			DynamicParameters p = new();

			string dateRead = book.DateRead.ToString("s");

			p.Add("@BookName", book.BookName);
			p.Add("@IsRead", book.IsRead ? 1 : 0);
			p.Add("@DateRead", dateRead);

			string sql = "insert into Book (BookName, IsRead, DateRead) values (@BookName, @IsRead, @DateRead); select last_insert_rowid()";

			book.Id = _saveData.ExecuteParametersReturnId(sql, p);

			if (book.Tags != null)
			{
				CreateBookTagRelationship(book);
			}
		}

		public void CreateBookTagRelationship(BookModel book)
		{
			Parallel.ForEach(book.Tags, tag =>
			{
				if (tag.Id == 0)
				{
					if (_sqliteTagData.IsTagInDatabase(tag, out int tagId))
					{
						tagId = tag.Id;
						AddBookTags(book.Id, tagId);
					}
					else
					{
						_sqliteTagData.CreateTag(tag);
					}
				}

				AddBookTags(book.Id, tag.Id);
			});
		}

		public void AddBookTags(int bookId, int tagId)
		{
			DynamicParameters p = new();

			p.Add("@BookId", bookId);
			p.Add("@TagId", tagId);

			string sql = "insert into Book_Tag (BookId, TagId) values (@BookId, @TagId)";

			_saveData.ExecuteParameters(sql, p);
		}
		public BookModel ReadBookById(int id)
		{
			string sql = $"select * from Book where id = {id}";

			BookModel output = _queryData.GetFirst(sql);

			output.Tags = _sqliteTagData.ReadTagsByBook(id);

			return output;
		}

		public List<BookModel> ReadAllBooks()
		{

			string sql = "select * from Book";
			List<BookModel> output = _queryData.GetList(sql);

			Parallel.ForEach(output, (book) => book.Tags = _sqliteTagData.ReadTagsByBook(book.Id));

			return output;
		}

		public List<BookModel> ReadBooksByTag(int tagId)
		{
			string sql = $"select * from Book where Book.Id In (select BookId from Book_Tag where TagId = {tagId})";

			List<BookModel> output = _queryData.GetList(sql);

			Parallel.ForEach(output, (book) => book.Tags = _sqliteTagData.ReadTagsByBook(book.Id));

			return output;
		}
		public void UpdateBook(BookModel book)
		{
			DynamicParameters p = new();

			string dateRead = book.DateRead.ToString("s");

			p.Add("@Id", book.Id);
			p.Add("@BookName", book.BookName);
			p.Add("@IsRead", book.IsRead ? 1 : 0);
			p.Add("@DateRead", dateRead);

			string sql = @"Update Book
                        set BookName = @BookName, IsRead = @IsRead, DateRead = @DateRead where Id = @Id";

			_saveData.ExecuteParameters(sql, p);

		}

		public void DeleteBook(BookModel book)
		{
			DeleteBookRelationship(book.Id);

			string sql = $"Delete from Book where id = {book.Id}";

			_saveData.ExecuteModel(sql, book);
		}

		public void DeleteBookRelationship(int bookId)
		{
			string sql = $"Delete from Book_Tag where BookId = {bookId}";

			_saveData.ExecuteId(sql, bookId);
		}
	}
}
