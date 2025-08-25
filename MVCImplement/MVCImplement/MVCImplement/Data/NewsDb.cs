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
            try
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
                        )";
                command.ExecuteNonQuery();

                command.CommandText = "SELECT COUNT(*) FROM News";
                var count = Convert.ToInt32(command.ExecuteScalar());
                if (count == 0)
                {
                    command.CommandText = @"
                            INSERT INTO News (Title, Content, CreatedAt)
                            VALUES ('Sample News 1', 'This is the content of Sample News 1', DATETIME('now')),
                                   ('Sample News 2', 'This is the content of Sample News 2', DATETIME('now'))";
                    command.ExecuteNonQuery();
                }

                command.CommandText = "SELECT * FROM News";
                using var reader = command.ExecuteReader();
                Console.WriteLine("News in database:");
                while (reader.Read())
                {
                    Console.WriteLine($"Id: {reader.GetInt32(0)}, Title: {reader.GetString(1)}, Content: {reader.GetString(2)}, CreatedAt: {reader.GetDateTime(3)}");
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"SqliteException in InitializeDatabase: {ex.Message}");
            }
        }

        public string ConnectionString => _connectionString;
    }
}
