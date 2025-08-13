using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.UserFileRepo;
using GoogleDriveUnittestWithDapper.Services.UserFileService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
