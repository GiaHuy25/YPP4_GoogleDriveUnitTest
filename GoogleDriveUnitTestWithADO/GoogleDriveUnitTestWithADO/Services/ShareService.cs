using GoogleDriveUnitTestWithADO.Database.Share;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnitTestWithADO.Services
{
    public class ShareService
    {
        private readonly IShareRepository _shareRepository;
        public ShareService(IShareRepository repository) 
        {
            _shareRepository = repository;
        }
        public int AddShare(Models.Share share)
        {
            share.CreatedAt = DateTime.Now;
            return _shareRepository.AddShare(share);
        }
        public void UpdateShare(Models.Share share)
        {
            share.CreatedAt = DateTime.Now;
            _shareRepository.UpdateShare(share);
        }
        public Models.Share GetShareById(int shareId)
        {
            return _shareRepository.GetShareById(shareId);
        }
        public void DeleteShare(int shareId)
        {
            _shareRepository.DeleteShare(shareId);
        }
    }
}
