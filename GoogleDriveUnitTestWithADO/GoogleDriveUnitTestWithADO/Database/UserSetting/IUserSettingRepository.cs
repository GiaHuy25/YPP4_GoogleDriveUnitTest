using GoogleDriveUnitTestWithADO.Models;
namespace GoogleDriveUnitTestWithADO.Database.UserSettingRepo
{
    public interface IUserSettingRepository
    {
        int AddUserSetting(UserSetting userSetting);
        UserSetting GetUserSettingById(int id);
        void UpdateUserSetting(UserSetting userSetting);
        void DeleteUserSetting(int userSettingId);
    }
}
