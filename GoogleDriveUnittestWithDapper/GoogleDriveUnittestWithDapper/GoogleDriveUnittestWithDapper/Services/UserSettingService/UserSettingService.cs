using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.UserSettingRepo;

namespace GoogleDriveUnittestWithDapper.Services.UserSettingService
{
    public class UserSettingService : IUserSettingService
    {
        private readonly IUserSettingRepository _userSettingRepository;

        public UserSettingService(IUserSettingRepository userSettingRepository)
        {
            _userSettingRepository = userSettingRepository;
        }

        public  IEnumerable<UserSettingDto> GetUserSettings(int userId)
        {
            return _userSettingRepository.GetUserSettings(userId);
        }
    }
}
