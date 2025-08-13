using GoogleDriveUnittestWithDapper.Dto;

namespace GoogleDriveUnittestWithDapper.Repositories.UserSettingRepo
{
    public interface IUserSettingRepository
    {
        IEnumerable<UserSettingDto> GetUserSettings(int userId);
    }
}
