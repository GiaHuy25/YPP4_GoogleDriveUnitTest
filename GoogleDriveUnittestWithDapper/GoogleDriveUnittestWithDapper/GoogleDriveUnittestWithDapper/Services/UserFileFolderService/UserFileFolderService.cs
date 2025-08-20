using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.UserFileFolderRepo;

namespace GoogleDriveUnittestWithDapper.Services.UserFileFolderService
{
    public class UserFileFolderService : IUserFileFolderService
    {
        private readonly IUserFileFolderRepository _userFileAndFolderRepository;
        private readonly Dictionary<int, IEnumerable<FavoriteObjectOfUserDto>> _favoriteCache = new();

        public UserFileFolderService(IUserFileFolderRepository userFileAndFolderRepository)
        {
            _userFileAndFolderRepository = userFileAndFolderRepository ?? throw new ArgumentNullException(nameof(userFileAndFolderRepository));
        }

        public IEnumerable<UserFileAndFolderDto> GetFilesAndFoldersByUserId(int userId)
        {
            _ = userId > 0 ? 0 : throw new ArgumentException("UserId must be a positive integer.", nameof(userId));
            return _userFileAndFolderRepository.GetFilesAndFoldersByUserId(userId) ?? new List<UserFileAndFolderDto>();
        }
        public IEnumerable<FileDto> GetFilesByUserId(int userId)
        {
            _ = userId > 0 ? 0 : throw new ArgumentException("UserId must be a positive integer.", nameof(userId));
            return _userFileAndFolderRepository.GetFilesByUserId(userId);
        }
        public IEnumerable<FolderDto>? GetFolderById(int folderId)
        {
            _ = folderId > 0 ? 0 : throw new ArgumentException("FolderId must be a positive integer.", nameof(folderId));
            return _userFileAndFolderRepository.GetFolderById(folderId);
        }
        public IEnumerable<FavoriteObjectOfUserDto> GetFavoritesByUserId(int userId)
        {
            _ = userId > 0 ? 0 : throw new ArgumentException("UserId must be a positive integer.", nameof(userId));

            return _favoriteCache.TryGetValue(userId, out var cachedFavorites)
                ? cachedFavorites
                : _favoriteCache[userId] = _userFileAndFolderRepository.GetFavoritesByUserId(userId);
        }
    }
}
