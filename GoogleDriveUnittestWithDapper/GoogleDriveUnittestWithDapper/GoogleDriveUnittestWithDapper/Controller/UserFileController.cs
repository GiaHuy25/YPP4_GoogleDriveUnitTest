using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Services.UserFileService;

namespace GoogleDriveUnittestWithDapper.Controller
{
    public class UserFileController
    {
        private readonly IUserFileService _userFileService;

        public UserFileController(IUserFileService userFileService)
        {
            _userFileService = userFileService ;
        }

        public IEnumerable<FileDto> GetFilesByUserId(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("UserId must be a positive integer.", nameof(userId));

            return _userFileService.GetFilesByUserId(userId);
        }
    }
}
