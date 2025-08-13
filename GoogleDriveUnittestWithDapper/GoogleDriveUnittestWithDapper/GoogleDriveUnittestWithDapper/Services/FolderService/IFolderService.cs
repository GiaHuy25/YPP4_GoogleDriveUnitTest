using GoogleDriveUnittestWithDapper.Dto;
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
        int CreateFolder(FolderDto folder);
        FolderDto? GetFolderById(int folderId);
    }
}
