using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.ShareObjectRepo;

namespace GoogleDriveUnittestWithDapper.Services.ShareObjectService
{
    public class ShareService : IShareService
    {
        private readonly IShareRepository _shareRepository;

        public ShareService(IShareRepository shareRepository)
        {
            _shareRepository = shareRepository;
        }

        public async Task<IEnumerable<ShareObjectDto>> GetSharedObjectsByUserIdAsync(int userId)
        {
            _ = userId > 0 ? 0 : throw new ArgumentException("UserId must be a positive integer.", nameof(userId));

            var sharedObjects = await _shareRepository.GetSharedObjectsByUserIdAsync(userId);
            return sharedObjects;
        }
    }
}
