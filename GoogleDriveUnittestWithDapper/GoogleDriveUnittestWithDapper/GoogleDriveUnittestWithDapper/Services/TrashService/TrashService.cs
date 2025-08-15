using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.TrashRepo;

namespace GoogleDriveUnittestWithDapper.Services.TrashService
{
    public class TrashService : ITrashService
    {
        private readonly ITrashRepository _trashRepository;

        public TrashService(ITrashRepository trashRepository)
        {
            _trashRepository = trashRepository;
        }

        public async Task<int> AddToTrashAsync(TrashDto trash)
        {
            if (string.IsNullOrEmpty(trash.FileName) && string.IsNullOrEmpty(trash.FolderName))
                throw new ArgumentException("Either FileName or FolderName must be provided.", nameof(trash));
            if (string.IsNullOrEmpty(trash.UserName))
                throw new ArgumentException("UserName is required.", nameof(trash));

            return await _trashRepository.AddToTrashAsync(trash);
        }

        public async Task<int> ClearTrashAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("UserId must be a positive integer.", nameof(userId));

            return await _trashRepository.ClearTrashAsync(userId);
        }

        public async Task<IEnumerable<TrashDto>> GetTrashByUserIdAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("UserId must be a positive integer.", nameof(userId));

            return await _trashRepository.GetTrashByUserIdAsync(userId);
        }
        public async Task<IEnumerable<TrashDto>> GetTrashByIdAsync(int trashId)
        {
            if (trashId < 0)
                throw new ArgumentException("TrashId cannot be negative.", nameof(trashId));

            return await _trashRepository.GetTrashByIdAsync(trashId);
        }
        public async Task<int> PermanentlyDeleteFromTrashAsync(int trashId, int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("UserId must be a positive integer.", nameof(userId));
            if (trashId <= 0)
                throw new ArgumentException("TrashId must be a positive integer.", nameof(trashId));

            return await _trashRepository.PermanentlyDeleteFromTrashAsync(trashId, userId);
        }

        public async Task<int> RestoreFromTrashAsync(int trashId, int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("UserId must be a positive integer.", nameof(userId));
            if (trashId <= 0)
                throw new ArgumentException("TrashId must be a positive integer.", nameof(trashId));

            return await _trashRepository.RestoreFromTrashAsync(trashId, userId);
        }
    }
}
