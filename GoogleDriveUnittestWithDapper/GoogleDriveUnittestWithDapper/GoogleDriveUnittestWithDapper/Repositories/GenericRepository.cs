using Dapper;
using GoogleDriveUnittestWithDapper.Dto;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace GoogleDriveUnittestWithDapper.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly string _connectionString;
        private readonly string _tableName;
        private readonly string _primaryKey;

        public GenericRepository(string connectionString, string tableName, string primaryKey = "Id")
        {
            _connectionString = connectionString;
            _tableName = tableName;
            _primaryKey = primaryKey;
        }

        private IDbConnection Connection => new SqliteConnection(_connectionString);

        public IQueryable<T> GetAllQueryable()
        {
            using (var connection = Connection)
            {
                var query = $"SELECT * FROM {_tableName}";
                var results = connection.Query<T>(query);
                return results.AsQueryable();
            }
        }

        public async Task<IQueryable<T>> GetAllQueryableAsync()
        {
            using (var connection = Connection)
            {
                var query = $"SELECT * FROM {_tableName}";
                var results = await connection.QueryAsync<T>(query);
                return results.AsQueryable();
            }
        }

        public async Task<T> GetByIdAsync(int id)
        {
            using (var connection = Connection)
            {
                var query = $"SELECT * FROM {_tableName} WHERE {_primaryKey} = @Id";
                return await connection.QuerySingleOrDefaultAsync<T>(query, new { Id = id });
            }
        }

        public async Task<int> AddAsync(T entity)
        {
            using (var connection = Connection)
            {
                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.Name != _primaryKey && p.GetValue(entity) != null)
                    .ToList();

                var columns = string.Join(", ", properties.Select(p => p.Name));
                var parameters = string.Join(", ", properties.Select(p => "@" + p.Name));
                var query = $"INSERT INTO {_tableName} ({columns}) VALUES ({parameters}); SELECT last_insert_rowid();";

                var param = properties.ToDictionary(
                    prop => prop.Name,
                    prop => prop.GetValue(entity)
                );

                return await connection.ExecuteScalarAsync<int>(query, param);
            }
        }

        public async Task<int> UpdateAsync(T entity)
        {
            using (var connection = Connection)
            {
                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.Name != _primaryKey && p.GetValue(entity) != null)
                    .ToList();

                var setClause = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));
                var query = $"UPDATE {_tableName} SET {setClause} WHERE {_primaryKey} = @{_primaryKey}";

                var param = properties.ToDictionary(
                    prop => prop.Name,
                    prop => prop.GetValue(entity)
                );

                var pkProperty = typeof(T).GetProperty(_primaryKey);
                if (pkProperty != null)
                {
                    param[_primaryKey] = pkProperty.GetValue(entity);
                }

                return await connection.ExecuteAsync(query, param);
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            using (var connection = Connection)
            {
                var query = $"DELETE FROM {_tableName} WHERE {_primaryKey} = @Id";
                return await connection.ExecuteAsync(query, new { Id = id });
            }
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> predicate)
        {
            using (var connection = Connection)
            {
                var query = $"SELECT * FROM {_tableName}";
                var results = connection.Query<T>(query);
                return results.AsQueryable().Where(predicate);
            }
        }

        public async Task<IQueryable<T>> FindByConditionAsync(Expression<Func<T, bool>> predicate)
        {
            using (var connection = Connection)
            {
                var (sql, parameters) = BuildWhereClause(predicate);
                var query = $"SELECT * FROM {_tableName} WHERE {sql}";
                var results = await connection.QueryAsync<T>(query, parameters);
                return results.AsQueryable();
            }
        }

        private (string sql, object parameters) BuildWhereClause(Expression<Func<T, bool>> predicate)
        {
            if (predicate.Body is BinaryExpression binaryExpression && binaryExpression.NodeType == ExpressionType.Equal)
            {
                var left = binaryExpression.Left as MemberExpression;
                var right = binaryExpression.Right as ConstantExpression;

                if (left != null && right != null)
                {
                    var propertyName = left.Member.Name;
                    var value = right.Value;
                    var paramName = $"@{propertyName}";
                    var sql = $"{propertyName} = {paramName}";
                    var parameters = new DynamicParameters();
                    parameters.Add(paramName, value);
                    return (sql, parameters);
                }
            }

            return ("1=1", new { });
        }

        public async Task<IQueryable<BannedUserDto>> GetBannedUsersByUserIdAsync(int userId)
        {
            using (var connection = Connection)
            {
                bool isSqlServer = connection.GetType().Name.Contains("SqlConnection");
                var noLock = isSqlServer ? "WITH (NOLOCK)" : "";
                var query = @"
                    SELECT 
                        bu.UserId,
                        bu.BannedAt,
                        bu.BannedUserId,
                        a.UserName AS BannedUserName
                    FROM BannedUser bu {noLock}
                    LEFT JOIN Account a {noLock} ON bu.BannedUserId = a.UserId
                    WHERE bu.UserId = @userId"
                    .Replace("{noLock}", noLock);

                var results = await connection.QueryAsync<BannedUserDto>(query, new { userId });
                return results.AsQueryable();
            }
        }
    }
}
