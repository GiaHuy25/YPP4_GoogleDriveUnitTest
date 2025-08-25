using Microsoft.Data.Sqlite;

namespace MVCImplement.Data
{
    public class NewsDb
    {
        private readonly string _connectionString = "Data Source=news.db";

        public NewsDb()
        {
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS News (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    Content TEXT NOT NULL,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                );
            ";
            command.ExecuteNonQuery();

            command.CommandText = @"
                INSERT INTO News (Title, Content, CreatedAt)
                SELECT 'Sample News 1', 'This is the content of Sample News 1', DATETIME('now')
                WHERE NOT EXISTS (SELECT 1 FROM News);
            ";
            command.ExecuteNonQuery();

            command.CommandText = @"
                INSERT INTO News (Title, Content, CreatedAt)
                SELECT 'Sample News 2', 'This is the content of Sample News 2', DATETIME('now')
                WHERE NOT EXISTS (SELECT 1 FROM News WHERE Title = 'Sample News 2');
            ";
            command.ExecuteNonQuery();
        }

        public string ConnectionString => _connectionString;
    }
}
