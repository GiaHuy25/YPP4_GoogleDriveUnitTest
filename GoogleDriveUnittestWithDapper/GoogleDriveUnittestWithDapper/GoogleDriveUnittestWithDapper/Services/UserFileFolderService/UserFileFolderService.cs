using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.UserFileFolderRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Services.UserFileFolderService
{
    public class UserFileFolderService : IUserFileFolderService
    {
        private readonly IUserFileFolderRepository _userFileAndFolderRepository;

        public UserFileFolderService(IUserFileFolderRepository userFileAndFolderRepository)
        {
            _userFileAndFolderRepository = userFileAndFolderRepository ?? throw new ArgumentNullException(nameof(userFileAndFolderRepository));
        }

        public IEnumerable<UserFileAndFolderDto> GetFilesAndFoldersByUserId(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("UserId must be a positive integer.", nameof(userId));

            return _userFileAndFolderRepository.GetFilesAndFoldersByUserId(userId) ?? new List<UserFileAndFolderDto>();
        }
    }
}
