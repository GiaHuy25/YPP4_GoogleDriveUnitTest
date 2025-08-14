using GoogleDriveUnittestWithDapper.Dto;

namespace GoogleDriveUnittestWithDapper.Repositories.FavoriteObjectRepo
{
    public interface IFavoriteObjectRepository
    {
        IEnumerable<FavoriteObjectOfUserDto> GetFavoritesByUserId(int userId);
    }
}
