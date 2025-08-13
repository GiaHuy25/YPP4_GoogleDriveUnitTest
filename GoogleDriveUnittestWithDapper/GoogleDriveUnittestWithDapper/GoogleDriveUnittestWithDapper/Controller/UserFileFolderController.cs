using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Services.UserFileFolderService;

namespace GoogleDriveUnittestWithDapper.Controller
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
            if (userId <= 0)
                throw new ArgumentException("UserId must be a positive integer.", nameof(userId));

            return _userFileAndFolderService.GetFilesAndFoldersByUserId(userId) ?? new List<UserFileAndFolderDto>();
        }
    }
}
