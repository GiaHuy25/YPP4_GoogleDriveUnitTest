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
            bool isSqlServer = _connection.GetType().Name.Contains("SqlConnection");
            var noLock = isSqlServer ? "WITH (NOLOCK)" : "";
            string sql = @"
                SELECT 
                    p.ProductName,
                    p.Cost,
                    p.Duration,
                    a.UserName,
                    pr.PromotionName,
                    pr.Discount,
                    pr.IsPercent
                FROM UserProduct up  {noLock}
                JOIN Account a {noLock} ON up.UserId = a.UserId
                JOIN ProductItem p {noLock} ON up.ProductId = p.ProductId
                LEFT JOIN Promotion pr {noLock} ON up.PromotionId = pr.PromotionId
                WHERE a.UserId = @UserId".Replace("{noLock}", noLock);

            return await _connection.QueryAsync<UserProductItemDto>(sql, new { UserId = userId });
        }

        public async Task<int> AddUserProductAsync(UserProductItemDto userProduct)
        {
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
