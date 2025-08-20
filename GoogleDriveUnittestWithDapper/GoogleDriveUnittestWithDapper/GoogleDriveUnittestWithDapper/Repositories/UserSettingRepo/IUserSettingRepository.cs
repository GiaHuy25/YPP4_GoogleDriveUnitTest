using GoogleDriveUnittestWithDapper.Models;

namespace GoogleDriveUnittestWithDapper.Repositories.UserSettingRepo
{
    public interface IUserSettingRepository
    {
        IQueryable<UserSetting> GetUserSettingsByUserId(int userId);
        IQueryable<AppSettingKey?> GetAppSettingKeyById(int settingId);
        IQueryable<AppSettingOption?> GetAppSettingOptionById(int optionId);

    }
}
