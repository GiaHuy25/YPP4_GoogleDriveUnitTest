using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.StorageRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (userId < 0)
                throw new ArgumentException("UserId cannot be negative.", nameof(userId));

            var storage = await _storageRepository.GetStorageByUserIdAsync(userId);
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

            if (fileSize > 1_073_741_824) 
                throw new ArgumentException("FileSize exceeds maximum allowed limit.", nameof(fileSize));

            return await _storageRepository.UpdateUsedCapacityAsync(userId, fileSize);
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

            return await _storageRepository.AddFileToStorageAsync(storage);
        }
    }
}
