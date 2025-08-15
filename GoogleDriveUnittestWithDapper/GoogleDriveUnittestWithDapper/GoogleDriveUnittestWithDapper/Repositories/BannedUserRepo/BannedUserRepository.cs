using Dapper;
using GoogleDriveUnittestWithDapper.Dto;
using System.Data;

namespace GoogleDriveUnittestWithDapper.Repositories.BannedUserRepo
{
    public class BannedUserRepository : IBannedUserRepository
    {
        private readonly IDbConnection _connection;
        public BannedUserRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public IEnumerable<BannedUserDto>GetBannedUserByUserId(int userId)
        {
            var query = @"
                    SELECT 
                        bu.UserId,
                        bu.BannedAt,
                        bu.BannedUserId,
                        a.UserName AS BannedUserName
                    FROM BannedUser bu  
                    LEFT JOIN Account a   ON bu.BannedUserId = a.UserId
                    WHERE bu.UserId = @userId";
            return _connection.Query<BannedUserDto>(query, new { userId });
        }
    }
}
