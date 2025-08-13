using GoogleDriveUnittestWithDapper.Dto;

namespace GoogleDriveUnittestWithDapper.Services.FavoriteObjectService
{
    public interface IFavoriteObjectService
    {
        IEnumerable<FavoriteObjectOfUser> GetFavoritesByUserId(int userId);
    }
}
