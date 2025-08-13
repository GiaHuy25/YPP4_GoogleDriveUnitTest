using GoogleDriveUnittestWithDapper.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Services.BannedUserService
{
    public interface IBannedUserService
    {
        Task<IEnumerable<BannedUserDto>> GetBannedUserByUserId(int userId);
    }
}
