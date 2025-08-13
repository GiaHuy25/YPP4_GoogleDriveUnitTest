using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.BannedUserRepo;
using GoogleDriveUnittestWithDapper.Repositories.UserSettingRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Services.BannedUserService
{
    public class BannedUserService : IBannedUserService
    {
        private readonly IBannedUserRepository _bannedUserRepository;
        public BannedUserService(IBannedUserRepository bannedUserRepository)
        {
            _bannedUserRepository = bannedUserRepository ?? throw new ArgumentNullException(nameof(bannedUserRepository));
        }
        public IEnumerable<BannedUserDto> GetBannedUserByUserId(int userId)
        {
            if (userId < 0) { 
                throw new ArgumentException(nameof(userId), "User ID cannot be negative.");
            }
            return _bannedUserRepository.GetBannedUserByUserId(userId);
        }
    }
}
