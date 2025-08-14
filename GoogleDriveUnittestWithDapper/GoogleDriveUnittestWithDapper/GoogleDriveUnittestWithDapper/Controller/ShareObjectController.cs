using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Services.ShareObjectService;

namespace GoogleDriveUnittestWithDapper.Controller
{
    public class ShareObjectController
    {
        private readonly IShareService _shareService;

        public ShareObjectController(IShareService shareService)
        {
            _shareService = shareService;
        }

        public async Task<IEnumerable<ShareObjectDto>> GetSharedObjectsByUserIdAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("UserId must be a positive integer.", nameof(userId));
            }

            var sharedObjects = await _shareService.GetSharedObjectsByUserIdAsync(userId);
            return sharedObjects;
        }
    }
}
