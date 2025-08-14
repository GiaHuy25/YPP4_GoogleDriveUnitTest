using GoogleDriveUnittestWithDapper.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Services.UserProductService
{
    public interface IUserProductService
    {
        Task<IEnumerable<UserProductItemDto>> GetUserProductsByUserIdAsync(int userId);
        Task<int> AddUserProductAsync(UserProductItemDto userProduct);
        Task<int> UpdateUserProductAsync(UserProductItemDto userProduct);
    }
}
