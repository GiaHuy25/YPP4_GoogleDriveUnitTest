using Dapper;
using GoogleDriveUnittestWithDapper.Models;
using System.Data;

namespace GoogleDriveUnittestWithDapper.Repositories.UserSettingRepo
{
    public class UserSettingRepository : IUserSettingRepository
    {
        private readonly IDbConnection _connection;
        private readonly Dictionary<int, IEnumerable<UserSetting>> _cache = new();
        private readonly Dictionary<int, AppSettingKey?> _appSettingKeyCache = new();
        private readonly Dictionary<int, AppSettingOption?> _appSettingOptionCache = new(); 
        public UserSettingRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<UserSetting> GetUserSettingsByUserId(int userId)
        {
            if (_cache.TryGetValue(userId, out var cachedResult))
            {
                return cachedResult;
            }
            bool isSqlServer = _connection.GetType().Name.Contains("SqlConnection");
            var noLock = isSqlServer ? "WITH (NOLOCK)" : "";
            var query = @"SELECT * 
                        FROM UserSetting {noLock}   
                        WHERE UserId = @userId".Replace("{noLock}", noLock);
            var result = _connection.Query<UserSetting>(query, new { userId });
            _cache[userId] = result.ToList();
            return result;
        }

        public AppSettingKey? GetAppSettingKeyById(int settingId)
        {
            if (_appSettingKeyCache.TryGetValue(settingId, out var cachedKey))
            {
                return cachedKey;
            }
            bool isSqlServer = _connection.GetType().Name.Contains("SqlConnection");
            var noLock = isSqlServer ? "WITH (NOLOCK)" : "";
            var query = @"SELECT * 
                        FROM AppSettingKey {noLock}   
                        WHERE SettingId = @settingId".Replace("{noLock}", noLock);
            var result = _connection.QuerySingleOrDefault<AppSettingKey>(query, new { settingId });
            if (result != null)
            {
                _appSettingKeyCache[settingId] = result;
            }
            return result;
        }

        public AppSettingOption? GetAppSettingOptionById(int optionId)
        {
            if (_appSettingOptionCache.TryGetValue(optionId, out var cachedOption))
            {
                return cachedOption;
            }
            bool isSqlServer = _connection.GetType().Name.Contains("SqlConnection");
            var noLock = isSqlServer ? "WITH (NOLOCK)" : "";
            var query = @"SELECT * 
                        FROM AppSettingOption {noLock} 
                        WHERE AppSettingOptionId = @optionId".Replace("{noLock}", noLock);
            var result = _connection.QuerySingleOrDefault<AppSettingOption>(query, new { optionId });
            if (result != null)
            {
                _appSettingOptionCache[optionId] = result;
            }
            return result;
        }
    }
}
