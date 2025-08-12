using GoogleDriveUnittestWithDapper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Repositories.FolderRepo
{
    public interface IFolderRepository
    {
        int CreateFolder(Folder folder);
        Folder? GetFolderById(int folderId);
    }
}
