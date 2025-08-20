using Dapper;
using GoogleDriveUnittestWithDapper.Dto;
using System.Data;

namespace GoogleDriveUnittestWithDapper.Repositories.BannedUserRepo
{
    public class BannedUserRepository : IBannedUserRepository
    {
        private readonly IDbConnection _connection;
        private readonly Dictionary<int, IEnumerable<BannedUserDto>> _cache = new();
        public BannedUserRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public IEnumerable<BannedUserDto>GetBannedUserByUserId(int userId)
        {
            if (_cache.TryGetValue(userId, out var cachedResult))
            {
                return cachedResult;
            }    
            bool isSqlServer = _connection.GetType().Name.Contains("SqlConnection");
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
            var result = _connection.Query<BannedUserDto>(query, new { userId });
            _cache[userId] = result.ToList();
            return result;
        }
    }
}
