using GoogleDriveUnitTestWithADO.Database.Folder;
using GoogleDriveUnitTestWithADO.Models;

namespace GoogleDriveUnitTestWithADO.Services
{
    public class FolderService
    {
        private readonly IFolderRepository _repository;
        public FolderService(IFolderRepository repository)
        {
            _repository = repository;
        }

        public Folder GetFolderById(int id)
        {
            return _repository.GetFolderById(id);
        }

        public int AddFolder(Folder folder)
        {
            folder.CreatedAt = DateTime.Now;
            return _repository.AddFolder(folder);
        }

        public void UpdateFolder(Folder folder)
        {
            folder.UpdatedAt = DateTime.Now;
            _repository.UpdateFolder(folder);
        }

        public void DeleteFolder(int folderId)
        {
            _repository.DeleteFolder(folderId);
        }
    }
}
