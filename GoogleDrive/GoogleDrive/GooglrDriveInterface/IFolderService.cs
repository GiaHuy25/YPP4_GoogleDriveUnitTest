using GoogleDrive.GoogleDriveModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDrive.GooglrDriveInterface
{
    public interface IFolderService
    {
        Task<Folder> CreateFolderAsync(string name, int ownerId, int? parentId);
        Task<Folder> GetFolderByIdAsync(int folderId);
        Task DeleteFolderAsync(int folderId);
    }
}
