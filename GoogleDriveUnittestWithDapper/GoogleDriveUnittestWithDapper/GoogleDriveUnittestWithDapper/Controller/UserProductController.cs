using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.UserProductRepo;
using GoogleDriveUnittestWithDapper.Services.UserProductService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Controller
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
            if (userId < 0)
                throw new ArgumentException("UserId cannot be negative.", nameof(userId));

            var products = await _userProductService.GetUserProductsByUserIdAsync(userId);
            if (products == null || !products.Any())
                throw new ArgumentException("No products found for the user.", nameof(userId));

            return products;
        }

        public async Task<int> AddUserProductAsync(UserProductItemDto userProduct)
        {
            if (userProduct == null)
                throw new ArgumentNullException(nameof(userProduct), "UserProduct object cannot be null.");
            if (string.IsNullOrEmpty(userProduct.UserName))
                throw new ArgumentException("UserName is required.", nameof(userProduct));
            if (string.IsNullOrEmpty(userProduct.ProductName))
                throw new ArgumentException("ProductName is required.", nameof(userProduct));
            if (userProduct.Cost < 0)
                throw new ArgumentException("Cost cannot be negative.", nameof(userProduct));
            if (userProduct.Duration < 0)
                throw new ArgumentException("Duration cannot be negative.", nameof(userProduct));

            return await _userProductService.AddUserProductAsync(userProduct);
        }

        public async Task<int> UpdateUserProductAsync(UserProductItemDto userProduct)
        {
            if (userProduct == null)
                throw new ArgumentNullException(nameof(userProduct), "UserProduct object cannot be null.");
            if (string.IsNullOrEmpty(userProduct.UserName))
                throw new ArgumentException("UserName is required.", nameof(userProduct));
            if (string.IsNullOrEmpty(userProduct.ProductName))
                throw new ArgumentException("ProductName is required.", nameof(userProduct));
            if (userProduct.Cost < 0)
                throw new ArgumentException("Cost cannot be negative.", nameof(userProduct));

            return await _userProductService.UpdateUserProductAsync(userProduct);
        }
    }
}
