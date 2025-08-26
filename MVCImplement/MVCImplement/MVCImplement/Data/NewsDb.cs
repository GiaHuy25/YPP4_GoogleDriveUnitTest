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

                command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS Users (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Username TEXT NOT NULL UNIQUE,
                            Password TEXT NOT NULL,
                            FullName TEXT NOT NULL,
                            CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                        )";
                command.ExecuteNonQuery();

                command.CommandText = "SELECT COUNT(*) FROM Users";
                var userCount = Convert.ToInt32(command.ExecuteScalar());
                if (userCount == 0)
                {
                    command.CommandText = @"
                            INSERT INTO Users (Username, Password, FullName, CreatedAt)
                            VALUES ('admin', '123456', 'Administrator', DATETIME('now')),
                                   ('user1', 'password1', 'User One', DATETIME('now')),
                                   ('user2', 'password2', 'User Two', DATETIME('now'))";
                    command.ExecuteNonQuery();
                }

                Console.WriteLine("News in database:");
                command.CommandText = "SELECT * FROM News";
                using var readerNews = command.ExecuteReader();
                while (readerNews.Read())
                {
                    Console.WriteLine($"Id: {readerNews.GetInt32(0)}, Title: {readerNews.GetString(1)}, Content: {readerNews.GetString(2)}, CreatedAt: {readerNews.GetDateTime(3)}");
                }

                Console.WriteLine("Users in database:");
                command.CommandText = "SELECT * FROM Users";
                using var readerUsers = command.ExecuteReader();
                while (readerUsers.Read())
                {
                    Console.WriteLine($"Id: {readerUsers.GetInt32(0)}, Username: {readerUsers.GetString(1)}, FullName: {readerUsers.GetString(3)}, CreatedAt: {readerUsers.GetDateTime(4)}");
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
