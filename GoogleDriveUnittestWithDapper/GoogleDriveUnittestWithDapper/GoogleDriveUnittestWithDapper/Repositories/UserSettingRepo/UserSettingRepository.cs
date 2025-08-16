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
            bool isSqlServer = _connection.GetType().Name.Contains("SqlConnection");
            var noLock = isSqlServer ? "WITH (NOLOCK)" : "";
            var query = @"SELECT * FROM UserSetting {noLock}   WHERE UserId = @userId".Replace("{noLock}", noLock);
            return _connection.Query<UserSetting>(query, new { userId });
        }

        public AppSettingKey? GetAppSettingKeyById(int settingId)
        {
            bool isSqlServer = _connection.GetType().Name.Contains("SqlConnection");
            var noLock = isSqlServer ? "WITH (NOLOCK)" : "";
            var query = @"SELECT * FROM AppSettingKey {noLock}   WHERE SettingId = @settingId".Replace("{noLock}", noLock);
            return _connection.QuerySingleOrDefault<AppSettingKey>(query, new { settingId });
        }

        public AppSettingOption? GetAppSettingOptionById(int optionId)
        {
            bool isSqlServer = _connection.GetType().Name.Contains("SqlConnection");
            var noLock = isSqlServer ? "WITH (NOLOCK)" : "";
            var query = @"SELECT * FROM AppSettingOption {noLock} WHERE AppSettingOptionId = @optionId".Replace("{noLock}", noLock);
            return _connection.QuerySingleOrDefault<AppSettingOption>(query, new { optionId });
        }
    }
}
