using Dapper;
using GoogleDriveUnittestWithDapper.Dto;
using System.Data;

namespace GoogleDriveUnittestWithDapper.Repositories.UserProductRepo
{
    public class UserProductRepository : IUserProductRepository
    {
        private readonly IDbConnection _connection;
        public UserProductRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public async Task<IEnumerable<UserProductItemDto>> GetUserProductsByUserIdAsync(int userId)
        {

            const string sql = @"
                SELECT 
                    p.ProductName,
                    p.Cost,
                    p.Duration,
                    a.UserName,
                    pr.PromotionName,
                    pr.Discount,
                    pr.IsPercent
                FROM UserProduct up
                JOIN Account a ON up.UserId = a.UserId
                JOIN ProductItem p ON up.ProductId = p.ProductId
                LEFT JOIN Promotion pr ON up.PromotionId = pr.PromotionId
                WHERE a.UserId = @UserId";

            return await _connection.QueryAsync<UserProductItemDto>(sql, new { UserId = userId });
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

            const string sql = @"
                INSERT INTO UserProduct (UserId, ProductId, PayingDatetime, IsFirstPaying, PromotionId, EndDatetime)
                VALUES (
                    (SELECT UserId FROM Account WHERE UserName = @UserName),
                    (SELECT ProductId FROM ProductItem WHERE ProductName = @ProductName),
                    datetime('now'),
                    0,
                    (SELECT PromotionId FROM Promotion WHERE PromotionName = @PromotionName LIMIT 1),
                    datetime('now', '+' || @Duration || ' days')
                );
                SELECT last_insert_rowid();";

            var parameters = new
            {
                UserName = userProduct.UserName,
                ProductName = userProduct.ProductName,
                Duration = userProduct.Duration,
                PromotionName = string.IsNullOrEmpty(userProduct.PromotionName) ? (object)DBNull.Value : userProduct.PromotionName
            };

            return await _connection.ExecuteScalarAsync<int>(sql, parameters);
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

            const string sql = @"
                UPDATE UserProduct up
                JOIN Account a ON up.UserId = a.UserId
                JOIN ProductItem p ON up.ProductId = p.ProductId
                SET p.Cost = @Cost,
                    up.EndDatetime = datetime('now', '+' || (SELECT Duration FROM ProductItem WHERE ProductId = up.ProductId) || ' days')
                WHERE a.UserName = @UserName AND p.ProductName = @ProductName;
                SELECT changes();";

            var parameters = new
            {
                UserName = userProduct.UserName,
                ProductName = userProduct.ProductName,
                Cost = userProduct.Cost
            };

            return await _connection.ExecuteScalarAsync<int>(sql, parameters);
        }
    }
}
