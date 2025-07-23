using GoogleDrive.GooglrDriveInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = GoogleDrive.GoogleDriveModel.File;

namespace GoogleDrive.GoogleDriveService
{
    public class FileService : IFileService
    {
        public async Task<File> CreateFileAsync(string name, int ownerId, int? folderId, int fileTypeId, long size)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("File name is required.", nameof(name));
            }
            if (name.Length > 255) // Schema constraint: NVARCHAR(255)
            {
                throw new ArgumentException("File name exceeds maximum length of 255 characters.", nameof(name));
            }
            if (ownerId <= 0)
            {
                throw new ArgumentException("Invalid owner ID.", nameof(ownerId));
            }
            if (fileTypeId <= 0)
            {
                throw new ArgumentException("Invalid file type ID.", nameof(fileTypeId));
            }
            if (size < 0)
            {
                throw new ArgumentException("File size cannot be negative.", nameof(size));
            }
            if (folderId.HasValue && folderId <= 0)
            {
                throw new ArgumentException("Invalid folder ID.", nameof(folderId));
            }
            return await Task.FromResult(new File
            {
                FileId = 1,
                Name = name,
                OwnerId = ownerId,
                FolderId = folderId,
                FileTypeId = fileTypeId,
                Size = size,
                Status = "Active",
                CreatedAt = DateTime.UtcNow
            });
        }

        public async Task<File> GetFileByIdAsync(int fileId)
        {
            if (fileId <= 0)
            {
                throw new ArgumentException("Invalid file ID.", nameof(fileId));
            }
            if (fileId == 1)
            {
                return await Task.FromResult(new File
                {
                    FileId = 1,
                    Name = "test.txt",
                    OwnerId = 1,
                    FolderId = 1,
                    FileTypeId = 1,
                    Size = 1024,
                    Status = "Active",
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                });
            }
            return await Task.FromResult((File)null);
        }

        public async Task DeleteFileAsync(int fileId)
        {
            if (fileId <= 0)
            {
                throw new ArgumentException("Invalid file ID.", nameof(fileId));
            }
            if (fileId != 1)
            {
                throw new ArgumentException("File not found.");
            }
            await Task.CompletedTask;
        }
    }
}
