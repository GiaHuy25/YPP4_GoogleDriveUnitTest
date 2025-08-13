using Dapper;
using GoogleDriveUnittestWithDapper.Dto;
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
        public IEnumerable<UserSettingDto> GetUserSettings(int userId)
        {
            var query = @"
                SELECT 
                    ak.SettingKey,
                    ak.IsBoolean,
                    ao.SettingValue
                FROM UserSetting us
                JOIN AppSettingKey ak ON us.AppSettingKeyId = ak.SettingId
                JOIN AppSettingOption ao ON us.AppSettingOptionId = ao.AppSettingOptionId
                WHERE us.UserId = @userId";

            return  _connection.Query<UserSettingDto>(query, new { userId });
        }
    }
}
