using GoogleDriveUnittestWithDapper.Models;
using GoogleDriveUnittestWithDapper.Repositories.FolderRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Services.FolderService
{
    public class FolderService : IFolderService
    {
        private readonly IFolderRepository _repository;

        public FolderService(IFolderRepository repository)
        {
            _repository = repository;
        }

        public Folder CreateFolder(string name, int ownerId)
        {
            var folder = new Folder
            {
                FolderName = name,
                OwnerId = ownerId,
                CreatedAt = DateTime.UtcNow
            };

            folder.FolderId = _repository.CreateFolder(folder);
            return folder;
        }
        public Folder? GetFolderById(int folderId)
        {
            return _repository.GetFolderById(folderId);
        }
    }
}
