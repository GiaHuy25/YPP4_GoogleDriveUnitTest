using Dapper;
using GoogleDriveUnittestWithDapper.Dto;
using System.Data;

namespace GoogleDriveUnittestWithDapper.Repositories.AccountRepo
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IDbConnection _connection;

        public AccountRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public AccountDto? GetUserByIdAsync(int userId)
        {
            bool isSqlServer = _connection.GetType().Name.Contains("SqlConnection");
            var noLock = isSqlServer ? "WITH (NOLOCK)" : "";
            var query = @"
                SELECT 
                    a.UserName AS UserName,
                    a.Email AS Email,
                    a.UserImg AS UserImg
                FROM Account a {noLock} 
                WHERE a.UserId = @userId".Replace("{noLock}", noLock);

            return  _connection.QuerySingleOrDefault<AccountDto>(query, new { userId });
        }
    }
}
