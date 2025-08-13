using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.FolderRepo;

namespace GoogleDriveUnittestWithDapper.Services.FolderService
{
    public class FolderService : IFolderService
    {
        private readonly IFolderRepository _repository;

        public FolderService(IFolderRepository repository)
        {
            _repository = repository;
        }

        public FolderDto? GetFolderById(int folderId)
        {
            if (folderId <= 0)
                throw new ArgumentException("FolderId must be a positive integer.", nameof(folderId));

            return _repository.GetFolderById(folderId);
        }
    }
}
