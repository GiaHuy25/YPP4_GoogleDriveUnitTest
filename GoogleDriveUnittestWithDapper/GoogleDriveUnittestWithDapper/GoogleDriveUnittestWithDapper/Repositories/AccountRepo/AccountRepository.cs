using Dapper;
using GoogleDriveUnittestWithDapper.Dto;
using System.Data;

namespace GoogleDriveUnittestWithDapper.Repositories.AccountRepo
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IDbConnection _connection;
        private readonly Dictionary<int, AccountDto> _cache = new();

        public AccountRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<AccountDto?> GetUserByIdAsync(int userId)
        {
            if (_cache.TryGetValue(userId, out var cachedUser))
            {
                return cachedUser;                
            }
            bool isSqlServer = _connection.GetType().Name.Contains("SqlConnection");
            var noLock = isSqlServer ? "WITH (NOLOCK)" : "";
            var query = @"
                SELECT 
                    a.UserName AS UserName,
                    a.Email AS Email,
                    a.UserImg AS UserImg
                FROM Account a {noLock} 
                WHERE a.UserId = @userId".Replace("{noLock}", noLock);

            var user = await _connection.QuerySingleOrDefaultAsync<AccountDto>(query, new { userId });
            if (user != null)
            {
                user.UserId = userId; 
                _cache[userId] = user; 
            }
            return user;
        }

        public async Task<CreateAccountDto> AddUserAsync(CreateAccountDto createAccountDto)
        {
            var query = @"
                INSERT INTO Account (UserName, Email, PasswordHash, UserImg)
                VALUES (@UserName, @Email, @PasswordHash, @UserImg);
                SELECT last_insert_rowid() AS UserId;";

            var parameters = new
            {
                createAccountDto.UserName,
                createAccountDto.Email,
                createAccountDto.PasswordHash,
                createAccountDto.UserImg
            };

            var userId = await _connection.ExecuteScalarAsync<int>(query, parameters);
            createAccountDto.UserId = userId;
            return createAccountDto;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var query = @"
                DELETE FROM Account 
                WHERE UserId = @userId;";

            var rowsAffected = await _connection.ExecuteAsync(query, new { userId });
            return rowsAffected > 0;
        }

        public async Task<AccountDto?> UpdateUserAsync(AccountDto accountDto)
        {
            var query = @"
                UPDATE Account 
                SET UserName = @UserName,
                    Email = @Email,
                    UserImg = @UserImg
                WHERE UserId = @UserId;";

            var rowsAffected = await _connection.ExecuteAsync(query, accountDto);
            return rowsAffected > 0 ? accountDto : null;
        }
    }
}
