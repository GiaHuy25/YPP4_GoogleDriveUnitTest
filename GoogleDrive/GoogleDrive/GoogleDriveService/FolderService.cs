using GoogleDrive.GoogleDriveModel;
using GoogleDrive.GoogleDriveInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoogleDrive.Repositories;

namespace GoogleDrive.GoogleDriveService
{
    public class FolderService : IFolderService
    {
        private readonly IGoogleDriveRepository _repository;

        public FolderService(IGoogleDriveRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public void CreateFolder(Folder folder)
        {
            if (folder == null)
                throw new ArgumentNullException(nameof(folder));
            if (string.IsNullOrWhiteSpace(folder.FolderName))
                throw new ArgumentException("FolderName is required.");
            if (folder.FolderName.Length > 50)
                throw new ArgumentException("FolderName cannot exceed 50 characters.");
            if (folder.OwnerId <= 0)
                throw new ArgumentException("Valid OwnerId is required.");
            if (!string.IsNullOrWhiteSpace(folder.FolderStatus) && folder.FolderStatus != "Active" && folder.FolderStatus != "Deleted")
                throw new ArgumentException("FolderStatus must be 'Active' or 'Deleted' if specified.");

            folder.CreatedAt = DateTime.UtcNow;
            folder.FolderStatus = folder.FolderStatus ?? "Active";
            _repository.AddFolder(folder);
        }

        public Folder GetFolderById(int folderId)
        {
            if (folderId <= 0)
                throw new ArgumentException("Invalid FolderId.");
            return _repository.GetFolderById(folderId);
        }

        public void UpdateFolder(Folder folder)
        {
            if (folder == null)
                throw new ArgumentNullException(nameof(folder));
            if (folder.FolderId <= 0)
                throw new ArgumentException("Invalid FolderId.");
            if (string.IsNullOrWhiteSpace(folder.FolderName))
                throw new ArgumentException("FolderName is required.");
            if (folder.FolderName.Length > 50)
                throw new ArgumentException("FolderName cannot exceed 50 characters.");
            if (folder.OwnerId <= 0)
                throw new ArgumentException("Valid OwnerId is required.");
            if (!string.IsNullOrWhiteSpace(folder.FolderStatus) && folder.FolderStatus != "Active" && folder.FolderStatus != "Deleted")
                throw new ArgumentException("FolderStatus must be 'Active' or 'Deleted' if specified.");

            var existingFolder = _repository.GetFolderById(folder.FolderId);
            if (existingFolder == null)
                throw new InvalidOperationException("Folder does not exist.");

            folder.FolderPath = folder.ParentId.HasValue ? $"{folder.ParentId}/{folder.FolderId}" : $"{folder.FolderId}";
            folder.UpdatedAt = DateTime.UtcNow;
            folder.CreatedAt = existingFolder.CreatedAt; 
            _repository.UpdateFolder(folder); 
        }
    }
}
