using GoogleDrive.GoogleDriveInterface;
using GoogleDrive.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserFile = GoogleDrive.GoogleDriveModel.UserFile;

namespace GoogleDrive.GoogleDriveService
{
    public class UserFileService : IUserFileService
    {
        private readonly IGoogleDriveRepository _repository;

        public UserFileService(IGoogleDriveRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public void CreateUserFile(UserFile file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));
            if (string.IsNullOrWhiteSpace(file.UserFileName) || string.IsNullOrWhiteSpace(file.UserFilePath))
                throw new ArgumentException("UserFileName and UserFilePath are required.");
            if (file.UserFileName.Length > 50)
                throw new ArgumentException("UserFileName cannot exceed 50 characters.");
            if (file.UserFilePath.Length > 50)
                throw new ArgumentException("UserFilePath cannot exceed 50 characters.");
            if (file.OwnerId <= 0)
                throw new ArgumentException("Valid OwnerId is required.");
            if (file.Size.HasValue && file.Size < 0)
                throw new ArgumentException("Size must be non-negative.");
            if (!string.IsNullOrWhiteSpace(file.UserFileStatus) && file.UserFileStatus != "Active" && file.UserFileStatus != "Deleted")
                throw new ArgumentException("UserFileStatus must be 'Active' or 'Deleted' if specified.");

            file.CreatedAt = DateTime.UtcNow;
            file.UserFileStatus = file.UserFileStatus ?? "Active";
            _repository.AddUserFile(file);
        }

        public UserFile GetUserFileById(int fileId)
        {
            if (fileId <= 0)
                throw new ArgumentException("Invalid FileId.");
            return _repository.GetUserFileById(fileId);
        }
    }
}
