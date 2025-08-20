using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.UserProductRepo;

namespace GoogleDriveUnittestWithDapper.Services.UserProductService
{
    public class UserProductService : IUserProductService
    {
        private readonly IUserProductRepository _userProductRepository;
        private readonly Dictionary<int, IEnumerable<UserProductItemDto>> _cache = new();

        public UserProductService(IUserProductRepository userProductRepository)
        {
            _userProductRepository = userProductRepository ?? throw new ArgumentNullException(nameof(userProductRepository));
        }

        public async Task<IEnumerable<UserProductItemDto>> GetUserProductsByUserIdAsync(int userId)
        {
            _ = userId >= 0 ? 0 : throw new ArgumentException("UserId cannot be negative.", nameof(userId));

            return await Task.FromResult(_cache.TryGetValue(userId, out var cachedResult)
                ? cachedResult
                : _cache[userId] = (await _userProductRepository.GetUserProductsByUserIdAsync(userId)).ToList())
                .ContinueWith(t => t.Result.Any() ? t.Result : throw new ArgumentException("No products found for the user.", nameof(userId)));
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
