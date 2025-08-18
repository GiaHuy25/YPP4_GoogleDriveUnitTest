using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.UserProductRepo;

namespace GoogleDriveUnittestWithDapper.Services.UserProductService
{
    public class UserProductService : IUserProductService
    {
        private readonly IUserProductRepository _userProductRepository;

        public UserProductService(IUserProductRepository userProductRepository)
        {
            _userProductRepository = userProductRepository ?? throw new ArgumentNullException(nameof(userProductRepository));
        }

        public async Task<IEnumerable<UserProductItemDto>> GetUserProductsByUserIdAsync(int userId)
        {
            _ = userId >= 0 ? 0 : throw new ArgumentException("UserId cannot be negative.", nameof(userId));
            var products = await _userProductRepository.GetUserProductsByUserIdAsync(userId);
            _ = products != null && products.Any() ? 0 : throw new ArgumentException("No products found for the user.", nameof(userId));
            return products;
        }

        public async Task<int> AddUserProductAsync(UserProductItemDto userProduct)
        {
            _ = userProduct != null ? 0 : throw new ArgumentNullException(nameof(userProduct), "UserProduct object cannot be null.");
            _ = !string.IsNullOrEmpty(userProduct.UserName) ? 0 : throw new ArgumentException("UserName is required.", nameof(userProduct));
            _ = !string.IsNullOrEmpty(userProduct.ProductName) ? 0 : throw new ArgumentException("ProductName is required.", nameof(userProduct));
            _ = userProduct.Cost >= 0 ? 0 : throw new ArgumentException("Cost cannot be negative.", nameof(userProduct));
            _ = userProduct.Duration >= 0 ? 0 : throw new ArgumentException("Duration cannot be negative.", nameof(userProduct));
            return await _userProductRepository.AddUserProductAsync(userProduct);
        }

        public async Task<int> UpdateUserProductAsync(UserProductItemDto userProduct)
        {
            _ = userProduct != null ? 0 : throw new ArgumentNullException(nameof(userProduct), "UserProduct object cannot be null.");
            _ = !string.IsNullOrEmpty(userProduct.UserName) ? 0 : throw new ArgumentException("UserName is required.", nameof(userProduct));
            _ = !string.IsNullOrEmpty(userProduct.ProductName) ? 0 : throw new ArgumentException("ProductName is required.", nameof(userProduct));
            _ = userProduct.Cost >= 0 ? 0 : throw new ArgumentException("Cost cannot be negative.", nameof(userProduct));
            return await _userProductRepository.UpdateUserProductAsync(userProduct);
        }
    }
}
