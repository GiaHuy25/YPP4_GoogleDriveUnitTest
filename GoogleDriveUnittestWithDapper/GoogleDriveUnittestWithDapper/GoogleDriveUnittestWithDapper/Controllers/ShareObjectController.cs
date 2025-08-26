using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Services.ShareObjectService;

namespace GoogleDriveUnittestWithDapper.Controllers
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
            var sharedObjects = await _shareService.GetSharedObjectsByUserIdAsync(userId);
            return sharedObjects;
        }
    }
}
