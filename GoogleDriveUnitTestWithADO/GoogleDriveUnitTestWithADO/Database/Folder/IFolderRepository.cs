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
        void AddFolder(Models.Folder folder);
    }
}
