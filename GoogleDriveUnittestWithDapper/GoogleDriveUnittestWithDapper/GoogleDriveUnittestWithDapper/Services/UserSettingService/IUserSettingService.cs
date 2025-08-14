using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Models;

namespace GoogleDriveUnittestWithDapper.Services.UserSettingService
{
    public interface IUserSettingService
    {
        IEnumerable<UserSettingDto> GetUserSettings(int userId);
    }
}
