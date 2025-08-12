using GoogleDriveUnitTestWithADO.Database.UserSettingRepo;
using GoogleDriveUnitTestWithADO.Models;
namespace GoogleDriveUnitTestWithADO.Services
{
    public class UserSettingService
    {
        private readonly IUserSettingRepository _userSettingRepository;
        public UserSettingService(IUserSettingRepository userSettingRepository)
        {
            _userSettingRepository = userSettingRepository;
        }
        public void AddUserSetting(UserSetting userSetting)
        {
            _userSettingRepository.AddUserSetting(userSetting);
        }
        public UserSetting GetUserSettingById(int id)
        {
            return _userSettingRepository.GetUserSettingById(id) as UserSetting;
        }
        public void UpdateUserSetting(UserSetting userSetting)
        {
            _userSettingRepository.UpdateUserSetting(userSetting);
        }
        public void DeleteUserSetting(int userSettingId)
        {
            _userSettingRepository.DeleteUserSetting(userSettingId);
        }
    }
}
