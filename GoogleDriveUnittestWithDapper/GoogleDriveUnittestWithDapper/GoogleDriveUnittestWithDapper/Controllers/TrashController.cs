using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Services.TrashService;

namespace GoogleDriveUnittestWithDapper.Controllers
{
    public class TrashController
    {
        private readonly ITrashService _trashService;

        public TrashController(ITrashService trashService)
        {
            _trashService = trashService;
        }
        public async Task<int> ClearTrashAsync(int userId)
        {
            return await _trashService.ClearTrashAsync(userId);
        }

        public async Task<IEnumerable<TrashDto>> GetTrashByUserIdAsync(int userId)
        {
            return await _trashService.GetTrashByUserIdAsync(userId);
        }
        public async Task<IEnumerable<TrashDto>> GetTrashByIdAsync(int trashId)
        {
            return await _trashService.GetTrashByIdAsync(trashId);
        }
        public async Task<int> AddToTrashAsync(TrashDto trash)
        {
           
            return await _trashService.AddToTrashAsync(trash);
        }

        public async Task<int> PermanentlyDeleteFromTrashAsync(int trashId, int userId)
        {
            return await _trashService.PermanentlyDeleteFromTrashAsync(trashId, userId);
        }

        public async Task<int> RestoreFromTrashAsync(int trashId, int userId)
        {
            return await _trashService.RestoreFromTrashAsync(trashId, userId);
        }
    }
}
