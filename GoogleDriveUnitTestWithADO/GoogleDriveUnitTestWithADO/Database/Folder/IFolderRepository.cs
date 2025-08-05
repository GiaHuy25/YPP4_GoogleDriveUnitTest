using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoogleDriveUnitTestWithADO.Models;

namespace GoogleDriveUnitTestWithADO.Database.Folder
{
    public interface IFolderRepository
    {
        Models.Folder GetFolderById(int id);
        int AddFolder(Models.Folder folder);
        void UpdateFolder(Models.Folder folder);
        void DeleteFolder(int id);
    }
}
