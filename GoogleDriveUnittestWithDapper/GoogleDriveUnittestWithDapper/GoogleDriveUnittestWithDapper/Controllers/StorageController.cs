using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Services.StorageService;

namespace GoogleDriveUnittestWithDapper.Controllers
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
            var storage = await _storageService.GetStorageByUserIdAsync(userId);
            return storage;
        }

        public async Task<int> UpdateUsedCapacityAsync(int userId, int fileSize)
        {
            return await _storageService.UpdateUsedCapacityAsync(userId, fileSize);
        }

        public async Task<int> AddFileToStorageAsync(StorageDto storage)
        {
            return await _storageService.AddFileToStorageAsync(storage);
        }
    }
}
