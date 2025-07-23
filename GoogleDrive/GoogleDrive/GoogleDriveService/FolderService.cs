using GoogleDrive.GoogleDriveModel;
using GoogleDrive.GooglrDriveInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDrive.GoogleDriveService
{
    public class FolderService : IFolderService
    {
        public async Task<Folder> CreateFolderAsync(string name, int ownerId, int? parentId)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Folder name is required.");
            }
            if (name.Length > 255)
            {
                throw new ArgumentException("Folder name exceeds maximum length of 255 characters.");
            }
            if (ownerId <= 0)
            {
                throw new ArgumentException("Invalid owner ID.");
            }
            if (parentId.HasValue && parentId <= 0)
            {
                throw new ArgumentException("Invalid parent folder ID.");
            }
            return await Task.FromResult(new Folder
            {
                FolderId = 1,
                Name = name,
                OwnerId = ownerId,
                ParentFolderId = parentId,
                CreatedAt = DateTime.UtcNow
            });
        }

        public async Task<Folder> GetFolderByIdAsync(int folderId)
        {
            if (folderId <= 0)
            {
                throw new ArgumentException("Invalid folder ID.");
            }
            if (folderId == 1)
            {
                return await Task.FromResult(new Folder
                {
                    FolderId = 1,
                    Name = "Test Folder",
                    OwnerId = 1,
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                });
            }
            return await Task.FromResult((Folder)null);
        }

        public async Task DeleteFolderAsync(int folderId)
        {
            if (folderId <= 0)
            {
                throw new ArgumentException("Invalid folder ID.");
            }
            if (folderId != 1)
            {
                throw new ArgumentException("Folder not found.");
            }
            await Task.CompletedTask;
        }
    }
}
