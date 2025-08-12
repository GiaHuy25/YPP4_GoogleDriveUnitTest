using Dapper;
using GoogleDriveUnittestWithDapper.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Repositories.AccountRepo
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IDbConnection _connection;

        public AccountRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public Task<IEnumerable<AccountDto>> GetBannedUser(int userId)
        {
            var query = @"
                SELECT 
                    BU.Id AS BanId,
                    Banned.UserName AS BannedUserName,
                    Banner.UserName AS BannedByUserName,
                    BU.BannedAt
                FROM BannedUser BU
                JOIN Account Banned ON BU.UserId = Banned.UserId
                JOIN Account Banner ON BU.BannedUserId = Banner.UserId
                where BU.UserId = @userId
                ORDER BY BU.BannedAt DESC";
            return _connection.QueryAsync<AccountDto>(query, new { userId });
        }

        public Task<AccountDto> GetUserByIdAsync(int userId)
        {
            var query = @"
                SELECT 
                    a.UserName AS UserName,
                    a.Email AS Email,
                    a.UserImg AS UserImg
                FROM Account a
                WHERE a.UserId = @userId";

            return  _connection.QueryFirstOrDefaultAsync<AccountDto>(query, new { userId });
        }
    }
}
