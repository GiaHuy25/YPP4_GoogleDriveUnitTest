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
            return _bannedService.GetBannedUserByUserId(userId);
        }
    }
}
