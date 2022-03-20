using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadingChecklistModels;
using Dapper;

namespace ReadingChecklistDataAccess
{
    public static class SqliteUpdater
    {
        public static void UpdateTag(TagModel tag)
        {
            using IDbConnection cnn = new SQLiteConnection(SqliteConnector.LoadConnectionString());

            var sql = "Update Tag set TagName = @TagName where Id = @Id";

            cnn.Execute(sql, tag);
        }

        public static void UpdateBook(BookModel book)
        {
            using IDbConnection cnn = new SQLiteConnection(SqliteConnector.LoadConnectionString());

            DynamicParameters p = new();

            string dateRead = book.DateRead.ToString("s");

            p.Add("@Id", book.Id);
            p.Add("@BookName", book.BookName);
            p.Add("@IsRead", book.IsRead ? 1 : 0);
            p.Add("@DateRead", dateRead);

            var sql = @"Update Book
                        set BookName = @BookName, IsRead = @IsRead, DateRead = @DateRead where Id = @Id";

            cnn.Execute(sql, p);

        }
    }
}
