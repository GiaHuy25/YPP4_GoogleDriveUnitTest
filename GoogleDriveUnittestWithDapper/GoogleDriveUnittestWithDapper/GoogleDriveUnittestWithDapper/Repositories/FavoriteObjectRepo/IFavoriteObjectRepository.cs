using GoogleDriveUnittestWithDapper.Dto;

namespace GoogleDriveUnittestWithDapper.Repositories.FavoriteObjectRepo
{
    public interface IFavoriteObjectRepository
    {
        IEnumerable<FavoriteObjectOfUser> GetFavoritesByUserId(int userId);
    }
}
