using Dapper;
using GoogleDriveUnittestWithDapper.Models;
using System.Data;

namespace GoogleDriveUnittestWithDapper.Repositories.UserSettingRepo
{
    public class UserSettingRepository : IUserSettingRepository
    {
        private readonly IDbConnection _connection;
        private readonly Dictionary<int, IQueryable<UserSetting>> _cache = new();
        private readonly Dictionary<int, AppSettingKey?> _appSettingKeyCache = new();
        private readonly Dictionary<int, AppSettingOption?> _appSettingOptionCache = new();

        public UserSettingRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public IQueryable<UserSetting> GetUserSettingsByUserId(int userId)
        {
            return _cache.TryGetValue(userId, out var cachedResult)
                ? cachedResult
                : (_cache[userId] = _connection.Query<UserSetting>(
                    @"SELECT * 
                      FROM UserSetting {noLock}   
                      WHERE UserId = @userId"
                    .Replace("{noLock}", _connection.GetType().Name.Contains("SqlConnection") ? "WITH (NOLOCK)" : ""),
                    new { userId }).AsQueryable());
        }

        public IQueryable<AppSettingKey?> GetAppSettingKeyById(int settingId)
        {
            return _appSettingKeyCache.TryGetValue(settingId, out var cachedKey)
                ? new List<AppSettingKey?> { cachedKey }.AsQueryable()
                : new List<AppSettingKey?> {
                    (_appSettingKeyCache[settingId] =
                        _connection.QuerySingleOrDefault<AppSettingKey>(
                            @"SELECT * 
                              FROM AppSettingKey {noLock}   
                              WHERE SettingId = @settingId"
                            .Replace("{noLock}", _connection.GetType().Name.Contains("SqlConnection") ? "WITH (NOLOCK)" : ""),
                            new { settingId }))
                  }.AsQueryable();
        }

        public IQueryable<AppSettingOption?> GetAppSettingOptionById(int optionId)
        {
            return _appSettingOptionCache.TryGetValue(optionId, out var cachedOption)
                ? new List<AppSettingOption?> { cachedOption }.AsQueryable()
                : new List<AppSettingOption?> {
                    (_appSettingOptionCache[optionId] =
                        _connection.QuerySingleOrDefault<AppSettingOption>(
                            @"SELECT * 
                              FROM AppSettingOption {noLock} 
                              WHERE AppSettingOptionId = @optionId"
                            .Replace("{noLock}", _connection.GetType().Name.Contains("SqlConnection") ? "WITH (NOLOCK)" : ""),
                            new { optionId }))
                  }.AsQueryable();
        }
    }
}
