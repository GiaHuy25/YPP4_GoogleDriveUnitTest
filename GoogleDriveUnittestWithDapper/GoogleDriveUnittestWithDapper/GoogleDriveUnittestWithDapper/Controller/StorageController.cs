using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Services.StorageService;

namespace GoogleDriveUnittestWithDapper.Controller
{
    public class StorageController
    {
        private readonly IStorageService _storageService;

        public StorageController(IStorageService storageService)
        {
            _storageService = storageService;
        }

        public async Task<IEnumerable<StorageDto>> GetStorageByUserIdAsync(int userId)
        {
            if (userId < 0)
                throw new ArgumentException("UserId cannot be negative.", nameof(userId));

            var storage = await _storageService.GetStorageByUserIdAsync(userId);
            if (storage == null || !storage.Any())
                throw new ArgumentException("No storage information found for the user.", nameof(userId));

            return storage;
        }

        public async Task<int> UpdateUsedCapacityAsync(int userId, int fileSize)
        {
            if (userId < 0)
                throw new ArgumentException("UserId cannot be negative.", nameof(userId));
            if (fileSize < 0)
                throw new ArgumentException("FileSize cannot be negative.", nameof(fileSize));

            if (fileSize > 1)
                throw new ArgumentException("FileSize exceeds maximum allowed limit.", nameof(fileSize));

            return await _storageService.UpdateUsedCapacityAsync(userId, fileSize);
        }

        public async Task<int> AddFileToStorageAsync(StorageDto storage)
        {
            if (storage == null)
                throw new ArgumentNullException(nameof(storage), "Storage object cannot be null.");
            if (string.IsNullOrEmpty(storage.FileName))
                throw new ArgumentException("FileName is required.", nameof(storage));
            if (string.IsNullOrEmpty(storage.FileType))
                throw new ArgumentException("FileType is required.", nameof(storage));
            if (storage.FileSize < 0)
                throw new ArgumentException("FileSize cannot be negative.", nameof(storage));
            if (storage.UserCapacity < 0)
                throw new ArgumentException("UserId cannot be negative.", nameof(storage.UserCapacity));

            var supportedTypes = new[] { "PDF", "Image", "Text" };
            if (!supportedTypes.Contains(storage.FileType))
                throw new ArgumentException("Unsupported file type.", nameof(storage.FileType));

            return await _storageService.AddFileToStorageAsync(storage);
        }
    }
}
