using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.FolderRepo;
using GoogleDriveUnittestWithDapper.Services.FolderService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Controller
{
    public class FolderController
    {
        private readonly IFolderService _folderService;

        public FolderController(IFolderService folderService)
        {
            _folderService = folderService;
        }

        public FolderDto? GetFolderById(int folderId)
        {
            if (folderId <= 0)
                throw new ArgumentException("FolderId must be a positive integer.", nameof(folderId));

            return _folderService.GetFolderById(folderId);
        }
    }
}
