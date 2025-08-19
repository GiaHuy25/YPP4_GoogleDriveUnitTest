using GoogleDriveUnittestWithDapper.Dto;

namespace GoogleDriveUnittestWithDapper.Services.UserSettingService
{
    public interface IUserSettingService
    {
        IEnumerable<UserSettingDto> GetUserSettings(int userId);
    }
}
