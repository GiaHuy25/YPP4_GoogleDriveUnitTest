using GoogleDriveUnittestWithDapper.Dto;

namespace GoogleDriveUnittestWithDapper.Repositories.UserProductRepo
{
    public interface IUserProductRepository
    {
        Task<IEnumerable<UserProductItemDto>> GetUserProductsByUserIdAsync(int userId);
        Task<int> AddUserProductAsync(UserProductItemDto userProduct);
        Task<int> UpdateUserProductAsync(UserProductItemDto userProduct);
    }
}
