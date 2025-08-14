using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Services.FavoriteObjectService;

namespace GoogleDriveUnittestWithDapper.Controller
{
    public class FavoriteObjectController
    {
        private readonly IFavoriteObjectService _favoriteObjectService;

        public FavoriteObjectController(IFavoriteObjectService favoriteObjectService)
        {
            _favoriteObjectService = favoriteObjectService;
        }

        public IEnumerable<FavoriteObjectOfUserDto> GetFavoritesByUserId(int userId)
        {
            return _favoriteObjectService.GetFavoritesByUserId(userId);
        }
    }
}
