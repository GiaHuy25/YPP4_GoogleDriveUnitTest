using GoogleDriveUnittestWithDapper.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Services.UserSettingService
{
    public interface IUserSettingService
    {
        Task<IEnumerable<UserSettingDto>> GetUserSettings(int userId);
    }
}
