using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.UserSettingRepo;
using GoogleDriveUnittestWithDapper.Services.UserSettingService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
