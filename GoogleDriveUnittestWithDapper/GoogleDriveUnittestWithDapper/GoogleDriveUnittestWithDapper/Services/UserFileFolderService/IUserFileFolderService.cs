using GoogleDriveUnittestWithDapper.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Services.UserFileFolderService
{
    public interface IUserFileFolderService
    {
        IEnumerable<UserFileAndFolderDto> GetFilesAndFoldersByUserId(int userId);
    }
}
