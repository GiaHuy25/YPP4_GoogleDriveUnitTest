using GoogleDriveUnittestWithDapper.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Repositories.BannedUserRepo
{
    public interface IBannedUserRepository
    {
        Task<IEnumerable<BannedUserDto>> GetBannedUserByUserId(int userId);
    }
}
