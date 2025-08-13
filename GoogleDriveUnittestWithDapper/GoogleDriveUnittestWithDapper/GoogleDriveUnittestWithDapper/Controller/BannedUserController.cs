using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Services.BannedUserService;

namespace GoogleDriveUnittestWithDapper.Controller
{
    public class BannedUserController
    {
        private readonly IBannedUserService _bannedService;

        public BannedUserController(IBannedUserService bannedService)
        {
            _bannedService = bannedService ?? throw new ArgumentNullException(nameof(bannedService));
        }

        public IEnumerable<BannedUserDto> GetBannedUserByUserId(int userId)
        {
            if (userId < 0)
            {
                throw new ArgumentException(nameof(userId), "User ID cannot be negative.");
            }
            return _bannedService.GetBannedUserByUserId(userId);
        }
    }
}
