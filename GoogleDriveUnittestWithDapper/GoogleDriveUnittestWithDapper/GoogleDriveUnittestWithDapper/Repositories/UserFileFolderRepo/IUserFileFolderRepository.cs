using GoogleDriveUnittestWithDapper.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Repositories.UserFileFolderRepo
{
    public interface IUserFileFolderRepository
    {
        IEnumerable<UserFileAndFolderDto> GetFilesAndFoldersByUserId(int userId);
    }
}
