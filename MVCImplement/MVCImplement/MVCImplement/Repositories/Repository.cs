using Dapper;
using Microsoft.Data.Sqlite;
using MVCImplement.Data;
using System.Data.Common;

namespace MVCImplement.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly string _connectionString;

        public Repository()
        {
            _connectionString = new NewsDb().ConnectionString;
        }

        public IQueryable<T> GetAll()
        {
            bool isSqlServer = _connectionString.GetType().Name.Contains("SqlConnection");
            var noLock = isSqlServer ? "WITH (NOLOCK)" : "";
            using var connection = new SqliteConnection(_connectionString);
            var tableName = typeof(T).Name;
            var query = $"SELECT * FROM {tableName} {noLock} ";
            var result = connection.Query<T>(query);
            return result.AsQueryable();
        }

        public IQueryable<T> GetById(int id)
        {
            bool isSqlServer = _connectionString.GetType().Name.Contains("SqlConnection");
            var noLock = isSqlServer ? "WITH (NOLOCK)" : "";
            using var connection = new SqliteConnection(_connectionString);
            var tableName = typeof(T).Name;
            var query = $"SELECT * FROM {tableName} {noLock} WHERE Id = @Id";
            var result = connection.Query<T>(query, new { Id = id });
            return result.AsQueryable();
        }

        public void Add(T entity)
        {
            bool isSqlServer = _connectionString.GetType().Name.Contains("SqlConnection");
            var noLock = isSqlServer ? "WITH (NOLOCK)" : "";
            using var connection = new SqliteConnection(_connectionString);
            var tableName = typeof(T).Name;
            var columns = string.Join(", ", typeof(T).GetProperties().Select(p => p.Name));
            var values = string.Join(", ", typeof(T).GetProperties().Select(p => $"@{p.Name}"));
            var query = $"INSERT INTO {tableName} {noLock} ({columns}) VALUES ({values})";
            connection.Execute(query, entity);
        }

        public void Update(T entity)
        {
            bool isSqlServer = _connectionString.GetType().Name.Contains("SqlConnection");
            var noLock = isSqlServer ? "WITH (NOLOCK)" : "";
            using var connection = new SqliteConnection(_connectionString);
            var tableName = typeof(T).Name;
            var setClause = string.Join(", ", typeof(T).GetProperties().Select(p => $"{p.Name} = @{p.Name}"));
            var query = $"UPDATE {tableName} {noLock} SET {setClause} WHERE Id = @Id";
            connection.Execute(query, entity);
        }

        public void Delete(int id)
        {
            bool isSqlServer = _connectionString.GetType().Name.Contains("SqlConnection");
            var noLock = isSqlServer ? "WITH (NOLOCK)" : "";
            using var connection = new SqliteConnection(_connectionString);
            var tableName = typeof(T).Name;
            var query = $"DELETE FROM {tableName} {noLock} WHERE Id = @Id";
            connection.Execute(query, new { Id = id });
        }
    }
}
