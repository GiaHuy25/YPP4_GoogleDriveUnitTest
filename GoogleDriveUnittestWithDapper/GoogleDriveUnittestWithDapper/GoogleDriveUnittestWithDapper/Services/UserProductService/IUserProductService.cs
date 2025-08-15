using GoogleDriveUnittestWithDapper.Dto;

namespace GoogleDriveUnittestWithDapper.Services.UserProductService
{
    public interface IUserProductService
    {
        Task<IEnumerable<UserProductItemDto>> GetUserProductsByUserIdAsync(int userId);
        Task<int> AddUserProductAsync(UserProductItemDto userProduct);
        Task<int> UpdateUserProductAsync(UserProductItemDto userProduct);
    }
}
