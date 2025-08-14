using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.FavoriteObjectRepo;

namespace GoogleDriveUnittestWithDapper.Services.FavoriteObjectService
{
    public class FavoriteObjectService : IFavoriteObjectService
    {
        private readonly IFavoriteObjectRepository _favoriteObjectRepository;

        public FavoriteObjectService(IFavoriteObjectRepository favoriteObjectRepository)
        {
            _favoriteObjectRepository = favoriteObjectRepository;
        }

        public IEnumerable<FavoriteObjectOfUserDto> GetFavoritesByUserId(int userId)
        {
            return _favoriteObjectRepository.GetFavoritesByUserId(userId);
        }
    }
}
