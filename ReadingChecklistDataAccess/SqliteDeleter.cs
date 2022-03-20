using ReadingChecklistModels;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace ReadingChecklistDataAccess
{
    public static class SqliteDeleter
    {
        public static void DeleteTag(TagModel tag)
        {
            using IDbConnection cnn = new SQLiteConnection(SqliteConnector.LoadConnectionString());

            DeleteTagRelationship(tag.Id);

            var sql = $"Delete from Tag where id = {tag.Id}";

            cnn.Execute(sql, tag);
        }

        public static void DeleteTagRelationship(int tagId)
        {
            using IDbConnection cnn = new SQLiteConnection(SqliteConnector.LoadConnectionString());

            string sql = $"Delete from Book_Tag where TagId = {tagId}";

            _ = cnn.Execute(sql, tagId);
        }

        public static void DeleteBook(BookModel book)
        {
            using IDbConnection cnn = new SQLiteConnection(SqliteConnector.LoadConnectionString());

            DeleteBookRelationship(book.Id);

            var sql = $"Delete from Book where id = {book.Id}";

            cnn.Execute(sql, book);
        }



        public static void DeleteBookRelationship(int bookId)
        {
            using IDbConnection cnn = new SQLiteConnection(SqliteConnector.LoadConnectionString());

            string sql = $"Delete from Book_Tag where BookId = {bookId}";

            _ = cnn.Execute(sql, bookId);
        }
    }
}
