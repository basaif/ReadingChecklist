using System.Data.SQLite;

namespace ReadingChecklistDataAccess
{
    public static class SqliteConnector
    {
        public static string LoadConnectionString(string id = "Default")
        {
            string appDir = $@"{AppDomain.CurrentDomain.BaseDirectory}\Database\";

            return new SQLiteConnection(@$"Data Source={appDir}ReadingChecklistDB.db;Version=3;").ConnectionString;

        }
    }
}