using GoogleDrive.GoogleDriveModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDrive.GoogleDriveInterface
{
    public interface IFolderService
    {
        void CreateFolder(Folder folder);
        Folder GetFolderById(int folderId);
        void UpdateFolder(Folder folder);
    }
}
