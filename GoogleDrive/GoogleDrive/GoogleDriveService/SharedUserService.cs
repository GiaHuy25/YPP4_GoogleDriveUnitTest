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
    public class SharedUserService : ISharedUserService
    {
        private readonly IGoogleDriveRepository _repository;

        public SharedUserService(IGoogleDriveRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public void CreateSharedUser(SharedUser sharedUser)
        {
            if (sharedUser == null)
                throw new ArgumentNullException(nameof(sharedUser));
            if (sharedUser.ShareId <= 0)
                throw new ArgumentException("Valid ShareId is required.");
            if (sharedUser.UserId <= 0)
                throw new ArgumentException("Valid UserId is required.");
            if (string.IsNullOrWhiteSpace(sharedUser.Permission) || (sharedUser.Permission != "Read" && sharedUser.Permission != "Write"))
                throw new ArgumentException("Permission must be 'Read' or 'Write'.");

            sharedUser.CreatedAt = DateTime.UtcNow;
            _repository.AddSharedUser(sharedUser);
        }

        public SharedUser GetSharedUserById(int sharedUserId)
        {
            if (sharedUserId <= 0)
                throw new ArgumentException("Invalid SharedUserId.");
            return _repository.GetSharedUserById(sharedUserId);
        }
    }
}
