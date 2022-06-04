using Dapper;
using Models.Library;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Library
{
	public static class SqliteCreater
	{
		public static void CreateTag(TagModel tag)
		{
			using IDbConnection cnn = new SQLiteConnection(SqliteConnector.LoadConnectionString());

			string sql = "insert into Tag (TagName) values (@TagName); select last_insert_rowid()";

			long returnedId = cnn.ExecuteScalar<long>(sql, tag);

			tag.Id = (int)returnedId;
		}

		public static void CreateBook(BookModel book)
		{
			using IDbConnection cnn = new SQLiteConnection(SqliteConnector.LoadConnectionString());

			DynamicParameters p = new();

			string dateRead = book.DateRead.ToString("s");

			p.Add("@BookName", book.BookName);
			p.Add("@IsRead", book.IsRead ? 1 : 0);
			p.Add("@DateRead", dateRead);

			string sql = "insert into Book (BookName, IsRead, DateRead) values (@BookName, @IsRead, @DateRead); select last_insert_rowid()";

			long returnedId = cnn.ExecuteScalar<long>(sql, p);

			book.Id = (int)returnedId;

			if (book.Tags != null)
			{
				CreateBookTagRelationship(book);
			}
		}

		public static void CreateBookTagRelationship(BookModel book)
		{
			Parallel.ForEach(book.Tags, tag =>
			{
				if (tag.Id == 0)
				{
					if (IsTagInDatabase(tag, out int tagId))
					{
						tagId = tag.Id;
						AddBookTags(book.Id, tagId);
					}
					else
					{
						CreateTag(tag);
					}
				}

				AddBookTags(book.Id, tag.Id);
			});
		}

		public static void AddBookTags(int bookId, int tagId)
		{
			using IDbConnection cnn = new SQLiteConnection(SqliteConnector.LoadConnectionString());
			DynamicParameters p = new();

			p.Add("@BookId", bookId);
			p.Add("@TagId", tagId);

			string sql = "insert into Book_Tag (BookId, TagId) values (@BookId, @TagId)";

			_ = cnn.Execute(sql, p);
		}

		private static bool IsTagInDatabase(TagModel tag, out int tagId)
		{
			List<TagModel> tags = SqliteReader.ReadAllTags();
			foreach (TagModel tagModel in tags)
			{
				if (tagModel.TagName == tag.TagName)
				{
					tagId = tagModel.Id;
					return true;
				}
			}
			tagId = 0;
			return false;
		}
	}
}
