using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Models;

namespace GoogleDriveUnittestWithDapper.Repositories.UserSettingRepo
{
    public interface IUserSettingRepository
    {
        IEnumerable<UserSetting> GetUserSettingsByUserId(int userId);
        AppSettingKey? GetAppSettingKeyById(int settingId);
        AppSettingOption? GetAppSettingOptionById(int optionId);

    }
}
