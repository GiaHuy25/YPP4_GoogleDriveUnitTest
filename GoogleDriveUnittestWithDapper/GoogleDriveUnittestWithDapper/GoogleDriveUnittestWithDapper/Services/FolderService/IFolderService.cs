using GoogleDriveUnittestWithDapper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Services.FolderService
{
    public interface IFolderService
    {
        Folder CreateFolder(string name, int ownerId);
        Folder? GetFolderById(int folderId);
    }
}
