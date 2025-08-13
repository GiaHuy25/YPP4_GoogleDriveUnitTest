using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Services.UserSettingService;

namespace GoogleDriveUnittestWithDapper.Controller
{
    public class UserSettingController
    {
        private readonly IUserSettingService _userSettingService;

        public UserSettingController(IUserSettingService userSettingService)
        {
            _userSettingService = userSettingService;
        }

        public IEnumerable<UserSettingDto> GetUserSettings(int userId)
        {
            return _userSettingService.GetUserSettings(userId);
        }
    }
}
