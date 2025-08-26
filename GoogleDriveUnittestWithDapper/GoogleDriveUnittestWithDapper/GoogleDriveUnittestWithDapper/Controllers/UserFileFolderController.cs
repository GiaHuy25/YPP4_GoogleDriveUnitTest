using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Services.UserFileFolderService;

namespace GoogleDriveUnittestWithDapper.Controllers
{
    public class UserFileFolderController
    {
        private readonly IUserFileFolderService _userFileAndFolderService;

        public UserFileFolderController(IUserFileFolderService userFileAndFolderService)
        {
                _userFileAndFolderService = userFileAndFolderService;
        }

        public IEnumerable<UserFileAndFolderDto> GetFilesAndFoldersByUserId(int userId)
        {
            return _userFileAndFolderService.GetFilesAndFoldersByUserId(userId) ?? new List<UserFileAndFolderDto>();
        }
        public IEnumerable<FileDto> GetFilesByUserId(int userId)
        {
            return _userFileAndFolderService.GetFilesByUserId(userId);
        }
        public IEnumerable<FolderDto> GetFolderById(int folderId)
        {
            return _userFileAndFolderService.GetFolderById(folderId);
        }
        public IEnumerable<FavoriteObjectOfUserDto> GetFavoritesByUserId(int userId)
        {
            return _userFileAndFolderService.GetFavoritesByUserId(userId);
        }
    }
}
