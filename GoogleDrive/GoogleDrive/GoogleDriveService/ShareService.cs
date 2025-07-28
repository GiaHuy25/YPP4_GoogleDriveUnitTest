using GoogleDrive.GoogleDriveInterface;
using GoogleDrive.GoogleDriveModel;
using GoogleDrive.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDrive.GoogleDriveService
{
    public class ShareService : IShareService
    {
        private readonly IGoogleDriveRepository _repository;

        public ShareService(IGoogleDriveRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public void CreateShare(Share share)
        {
            if (share == null)
                throw new ArgumentNullException(nameof(share));
            if (string.IsNullOrWhiteSpace(share.ShareUrl))
                throw new ArgumentException("ShareUrl is required.");
            if (share.ShareUrl.Length > 50)
                throw new ArgumentException("ShareUrl cannot exceed 50 characters.");
            if (share.Sharer <= 0)
                throw new ArgumentException("Valid Sharer is required.");
            if (share.ObjectId <= 0)
                throw new ArgumentException("Valid ObjectId is required.");
            if (share.ObjectTypeId <= 0)
                throw new ArgumentException("Valid ObjectTypeId is required.");

            share.CreatedAt = DateTime.UtcNow;
            share.UrlApprove ??= false;
            _repository.AddShare(share);
        }

        public Share GetShareById(int shareId)
        {
            if (shareId <= 0)
                throw new ArgumentException("Invalid ShareId.");
            return _repository.GetShareById(shareId);
        }
    }
}
