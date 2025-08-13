using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.UserSettingRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
