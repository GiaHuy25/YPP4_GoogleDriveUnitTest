using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnitTestWithADO.Database.UserSetting
{
    public interface IUserSettingRepository
    {
        int AddUserSetting(Models.UserSetting userSetting);
        Models.UserSetting GetUserSettingById(int id);
        void UpdateUserSetting(Models.UserSetting userSetting);
        void DeleteUserSetting(int userSettingId);
    }
}
