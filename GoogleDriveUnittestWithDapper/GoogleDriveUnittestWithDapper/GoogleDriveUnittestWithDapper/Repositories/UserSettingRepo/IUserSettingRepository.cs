using GoogleDriveUnittestWithDapper.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Repositories.UserSettingRepo
{
    public interface IUserSettingRepository
    {
        Task<IEnumerable<UserSettingDto>> GetUserSettings(int userId);
    }
}
