using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoogleDrive.GoogleDriveModel;
using File = GoogleDrive.GoogleDriveModel.File;

namespace GoogleDrive.GooglrDriveInterface
{
    public interface IFileService
    {
        Task<File> CreateFileAsync(string name, int ownerId, int? folderId, int fileTypeId, long size);
        Task<File> GetFileByIdAsync(int fileId);
        Task DeleteFileAsync(int fileId);
    }
}
