using GoogleDriveUnitTestWithADO.Database.ShareRepo;
using GoogleDriveUnitTestWithADO.Models;

namespace GoogleDriveUnitTestWithADO.Services
{
    public class ShareService
    {
        private readonly IShareRepository _shareRepository;
        public ShareService(IShareRepository repository) 
        {
            _shareRepository = repository;
        }
        public int AddShare(Share share)
        {
            share.CreatedAt = DateTime.Now;
            return _shareRepository.AddShare(share);
        }
        public void UpdateShare(Share share)
        {
            share.CreatedAt = DateTime.Now;
            _shareRepository.UpdateShare(share);
        }
        public Share GetShareById(int shareId)
        {
            return _shareRepository.GetShareById(shareId);
        }
        public void DeleteShare(int shareId)
        {
            _shareRepository.DeleteShare(shareId);
        }
    }
}
