using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Services.UserProductService;

namespace GoogleDriveUnittestWithDapper.Controllers
{
    public class UserProductController
    {
        private readonly IUserProductService _userProductService;

        public UserProductController(IUserProductService userProductService)
        {
            _userProductService = userProductService ;
        }

        public async Task<IEnumerable<UserProductItemDto>> GetUserProductsByUserIdAsync(int userId)
        {
            var products = await _userProductService.GetUserProductsByUserIdAsync(userId);
            return products;
        }

        public async Task<int> AddUserProductAsync(UserProductItemDto? userProduct)
        {
            return await _userProductService.AddUserProductAsync(userProduct);
        }

        public async Task<int> UpdateUserProductAsync(UserProductItemDto? userProduct)
        {
            return await _userProductService.UpdateUserProductAsync(userProduct);
        }
    }
}
