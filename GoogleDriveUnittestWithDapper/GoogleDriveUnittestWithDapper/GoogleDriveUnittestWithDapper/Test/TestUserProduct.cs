using GoogleDriveUnittestWithDapper.Controller;
using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.UserProductRepo;
using GoogleDriveUnittestWithDapper.Services.UserProductService;
using Microsoft.Data.Sqlite;
using System.Data;

namespace GoogleDriveUnittestWithDapper.Test
{
    [TestClass]
    public class TestUserProduct
    {
        private IDbConnection? _dbConnection; 
        private IUserProductRepository? _userProductRepository; 
        private IUserProductService? _userProductService; 
        private UserProductController? _userProductController;

        [TestInitialize]
        public void Setup()
        {
            _dbConnection = new SqliteConnection("Data Source=:memory:");
            _dbConnection.Open();
            TestDatabaseSchema.CreateSchema(_dbConnection);
            TestDatabaseSchema.InsertSampleData(_dbConnection);

            _userProductRepository = new UserProductRepository(_dbConnection);
            _userProductService = new UserProductService(_userProductRepository);
            _userProductController = new UserProductController(_userProductService);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbConnection?.Close();
            _dbConnection?.Dispose();
        }
        [TestMethod]
        public async Task GetUserProductsByUserIdAsync_ValidUserId_ReturnsMultipleUserProductItemDtos()
        {
            // Arrange
            int userId = 1; // John

            // Act
            var result = await _userProductService!.GetUserProductsByUserIdAsync(userId); 

            // Assert
            Assert.IsNotNull(result, "Result should not be null.");
            Assert.IsTrue(result.Any(), "Result should contain products.");
            Assert.AreEqual(3, result.Count(), "Should return 3 products for John.");
            var products = result.ToList();
            Assert.IsTrue(products.Any(p => p.ProductName == "Plan1" && p.Cost == 9.99 && p.Duration == 30)); 
            Assert.IsTrue(products.Any(p => p.ProductName == "Plan2" && p.Cost == 19.99 && p.Duration == 60));
            Assert.IsTrue(products.Any(p => p.ProductName == "Plan3" && p.Cost == 29.99 && p.Duration == 90));
            Assert.AreEqual("John", products[0].UserName, "UserName should match.");
        }

        [TestMethod]
        public void GetUserProductsByUserNameAsync_NegativeUserId_ThrowsArgumentException()
        {
            // Arrange
            int invalidUserId = -1;

            // Act & Assert
            try
            {
                _userProductController!.GetUserProductsByUserIdAsync(invalidUserId).Wait();
                Assert.Fail("Should have thrown ArgumentException.");
            }
            catch (AggregateException ex)
            {
                Assert.IsInstanceOfType(ex.InnerException, typeof(ArgumentException), "Should throw ArgumentException.");
            }
        }

        [TestMethod]
        public void AddUserProductAsync_NegativeCost_ThrowsArgumentException()
        {
            // Arrange
            var userProduct = new UserProductItemDto
            {
                UserName = "John",
                ProductName = "Plan4",
                Cost = -1,
                Duration = 30
            };

            // Act & Assert
            try
            {
                _userProductController!.AddUserProductAsync(userProduct).Wait();
                Assert.Fail("Should have thrown ArgumentException.");
            }
            catch (AggregateException ex)
            {
                Assert.IsInstanceOfType(ex.InnerException, typeof(ArgumentException), "Should throw ArgumentException.");
            }
        }

        [TestMethod]
        public void AddUserProductAsync_NullUserProduct_ThrowsArgumentException()
        {
            // Arrange
            UserProductItemDto? userProduct = null;

            // Act & Assert
            try
            {
                _userProductController!.AddUserProductAsync(userProduct).Wait();
                Assert.Fail("Should have thrown ArgumentNullException.");
            }
            catch (AggregateException ex)
            {
                Assert.IsInstanceOfType(ex.InnerException, typeof(ArgumentNullException), "Should throw ArgumentNullException.");
            }
        }

        [TestMethod]
        public void UpdateUserProductAsync_NegativeCost_ThrowsArgumentException()
        {
            // Arrange
            var userProduct = new UserProductItemDto
            {
                UserName = "John",
                ProductName = "Plan1",
                Cost = -1
            };

            // Act & Assert
            try
            {
                _userProductController!.UpdateUserProductAsync(userProduct).Wait();
                Assert.Fail("Should have thrown ArgumentException.");
            }
            catch (AggregateException ex)
            {
                Assert.IsInstanceOfType(ex.InnerException, typeof(ArgumentException), "Should throw ArgumentException.");
            }
        }

        [TestMethod]
        public void UpdateUserProductAsync_NullUserProduct_ThrowsArgumentException()
        {
            // Arrange
            UserProductItemDto? userProduct = null;

            // Act & Assert
            try
            {
                _userProductController!.UpdateUserProductAsync(userProduct).Wait();
                Assert.Fail("Should have thrown ArgumentNullException.");
            }
            catch (AggregateException ex)
            {
                Assert.IsInstanceOfType(ex.InnerException, typeof(ArgumentNullException), "Should throw ArgumentNullException.");
            }
        }
    }
}
