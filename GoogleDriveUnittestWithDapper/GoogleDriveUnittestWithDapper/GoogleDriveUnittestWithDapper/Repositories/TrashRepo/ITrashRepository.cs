using GoogleDriveUnittestWithDapper.Dto;

namespace GoogleDriveUnittestWithDapper.Repositories.TrashRepo
{
    public interface ITrashRepository
    {
        Task<IEnumerable<TrashDto>> GetTrashByUserIdAsync(int userId);
        Task<int> RestoreFromTrashAsync(int trashId, int userId);
        Task<int> PermanentlyDeleteFromTrashAsync(int trashId, int userId);
        Task<int> ClearTrashAsync(int userId);
        Task<int> AddToTrashAsync(TrashDto trash);
        Task<IEnumerable<TrashDto>> GetTrashByIdAsync(int trashId);
    }
}
