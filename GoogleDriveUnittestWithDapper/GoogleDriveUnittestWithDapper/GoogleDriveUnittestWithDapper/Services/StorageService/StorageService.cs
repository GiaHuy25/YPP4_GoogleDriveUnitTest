using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.StorageRepo;

namespace GoogleDriveUnittestWithDapper.Services.StorageService
{
    public class StorageService : IStorageService
    {
        private readonly IStorageRepository _storageRepository;

        public StorageService(IStorageRepository storageRepository)
        {
            _storageRepository = storageRepository;
        }

        public async Task<IEnumerable<StorageDto>> GetStorageByUserIdAsync(int userId)
        {
            _ = userId >= 0 ? 0 : throw new ArgumentException("UserId cannot be negative.", nameof(userId));
            var storage = await _storageRepository.GetStorageByUserIdAsync(userId);
            _ = storage != null && storage.Any() ? 0 : throw new ArgumentException("No storage information found for the user.", nameof(userId));
            return storage;
        }

        public async Task<int> UpdateUsedCapacityAsync(int userId, int fileSize)
        {
            _ = userId >= 0 ? 0 : throw new ArgumentException("UserId cannot be negative.", nameof(userId));
            _ = fileSize >= 0 ? 0 : throw new ArgumentException("FileSize cannot be negative.", nameof(fileSize));
            _ = fileSize <= 1_073_741_824 ? 0 : throw new ArgumentException("FileSize exceeds maximum allowed limit.", nameof(fileSize));
            return await _storageRepository.UpdateUsedCapacityAsync(userId, fileSize);
        }

        public async Task<int> AddFileToStorageAsync(StorageDto storage)
        {
            _ = storage != null ? 0 : throw new ArgumentNullException(nameof(storage), "Storage object cannot be null.");
            _ = !string.IsNullOrEmpty(storage.FileName) ? 0 : throw new ArgumentException("FileName is required.", nameof(storage));
            _ = !string.IsNullOrEmpty(storage.FileType) ? 0 : throw new ArgumentException("FileType is required.", nameof(storage));
            _ = storage.FileSize >= 0 ? 0 : throw new ArgumentException("FileSize cannot be negative.", nameof(storage));
            _ = storage.UserCapacity >= 0 ? 0 : throw new ArgumentException("UserId cannot be negative.", nameof(storage.UserCapacity));
            var supportedTypes = new[] { "PDF", "Image", "Text" };
            _ = supportedTypes.Contains(storage.FileType) ? 0 : throw new ArgumentException("Unsupported file type.", nameof(storage.FileType));
            return await _storageRepository.AddFileToStorageAsync(storage);
        }
    }
}
