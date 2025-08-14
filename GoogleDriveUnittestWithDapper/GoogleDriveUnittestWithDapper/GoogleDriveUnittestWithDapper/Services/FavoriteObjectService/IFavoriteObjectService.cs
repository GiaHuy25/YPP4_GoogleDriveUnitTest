using GoogleDriveUnittestWithDapper.Dto;

namespace GoogleDriveUnittestWithDapper.Services.FavoriteObjectService
{
    public interface IFavoriteObjectService
    {
        IEnumerable<FavoriteObjectOfUserDto> GetFavoritesByUserId(int userId);
    }
}
