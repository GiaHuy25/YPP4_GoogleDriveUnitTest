using Dapper;
using GoogleDriveUnittestWithDapper.Models;
using System.Data;

namespace GoogleDriveUnittestWithDapper.Repositories.UserSettingRepo
{
    public class UserSettingRepository : IUserSettingRepository
    {
        private readonly IDbConnection _connection;
        public UserSettingRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<UserSetting> GetUserSettingsByUserId(int userId)
        {
            var query = @"SELECT * FROM UserSetting WHERE UserId = @userId";
            return _connection.Query<UserSetting>(query, new { userId });
        }

        public AppSettingKey? GetAppSettingKeyById(int settingId)
        {
            var query = @"SELECT * FROM AppSettingKey WHERE SettingId = @settingId";
            return _connection.QuerySingleOrDefault<AppSettingKey>(query, new { settingId });
        }

        public AppSettingOption? GetAppSettingOptionById(int optionId)
        {
            var query = @"SELECT * FROM AppSettingOption WHERE AppSettingOptionId = @optionId";
            return _connection.QuerySingleOrDefault<AppSettingOption>(query, new { optionId });
        }
    }
}
