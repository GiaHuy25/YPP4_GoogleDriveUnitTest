using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.UserProductRepo;
using GoogleDriveUnittestWithDapper.Services.UserProductService;
using Moq;

namespace GoogleDriveUnittestWithDapper.Test
{
    [TestClass]
    public class TestUserProduct
    {
        private Mock<IUserProductRepository>? _mockRepo;
        private UserProductService? _service;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IUserProductRepository>();

            // Mock GetUserProductsByUserIdAsync
            _mockRepo.Setup(r => r.GetUserProductsByUserIdAsync(1))
                .ReturnsAsync(new List<UserProductItemDto>
                {
                    new() { ProductName = "Plan1", Cost = 9.99, Duration = 30, UserName = "John" },
                    new() { ProductName = "Plan2", Cost = 19.99, Duration = 60, UserName = "John" },
                    new() { ProductName = "Plan3", Cost = 29.99, Duration = 90, UserName = "John" }
                });

            _mockRepo.Setup(r => r.GetUserProductsByUserIdAsync(It.Is<int>(id => id != 1)))
                .ReturnsAsync(new List<UserProductItemDto>());

            // Mock AddUserProductAsync
            _mockRepo.Setup(r => r.AddUserProductAsync(It.IsAny<UserProductItemDto>()))
                .ReturnsAsync(123);

            // Mock UpdateUserProductAsync
            _mockRepo.Setup(r => r.UpdateUserProductAsync(It.IsAny<UserProductItemDto>()))
                .ReturnsAsync(1);

            _service = new UserProductService(_mockRepo.Object);
        }

        [TestMethod]
        public async Task GetUserProductsByUserIdAsync_ValidUserId_ReturnsProducts()
        {
            var result = (await _service!.GetUserProductsByUserIdAsync(1)).ToList();

            Assert.HasCount(3, result);
            Assert.IsTrue(result.Any(p => p.ProductName == "Plan1" && p.Cost == 9.99 && p.Duration == 30));
        }

        [TestMethod]
        public async Task GetUserProductsByUserIdAsync_NegativeUserId_ThrowsException()
        {
            try
            {
                await _service!.GetUserProductsByUserIdAsync(-1);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "userId");
            }
        }

        [TestMethod]
        public async Task GetUserProductsByUserIdAsync_NoProductsFound_ThrowsException()
        {
            try
            {
                await _service!.GetUserProductsByUserIdAsync(999);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "No products found");
            }
        }

        [TestMethod]
        public async Task AddUserProductAsync_ValidProduct_ReturnsId()
        {
            var userProduct = new UserProductItemDto
            {
                UserName = "John",
                ProductName = "PlanX",
                Cost = 49.99,
                Duration = 120
            };

            var id = await _service!.AddUserProductAsync(userProduct);

            Assert.AreEqual(123, id);
        }

        [TestMethod]
        public async Task AddUserProductAsync_NullUserProduct_ThrowsException()
        {
            try
            {
                await _service!.AddUserProductAsync(null!);
                Assert.Fail("Expected ArgumentNullException was not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains(ex.Message, "userProduct");
            }
        }

        [TestMethod]
        public async Task AddUserProductAsync_NegativeCost_ThrowsException()
        {
            var userProduct = new UserProductItemDto
            {
                UserName = "John",
                ProductName = "PlanX",
                Cost = -1,
                Duration = 30
            };

            try
            {
                await _service!.AddUserProductAsync(userProduct);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "Cost");
            }
        }

        [TestMethod]
        public async Task UpdateUserProductAsync_ValidProduct_ReturnsRowsAffected()
        {
            var userProduct = new UserProductItemDto
            {
                UserName = "John",
                ProductName = "Plan1",
                Cost = 15.99
            };

            var rows = await _service!.UpdateUserProductAsync(userProduct);

            Assert.AreEqual(1, rows);
        }

        [TestMethod]
        public async Task UpdateUserProductAsync_NullUserProduct_ThrowsException()
        {
            try
            {
                await _service!.UpdateUserProductAsync(null!);
                Assert.Fail("Expected ArgumentNullException was not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains(ex.Message, "userProduct");
            }
        }

        [TestMethod]
        public async Task UpdateUserProductAsync_NegativeCost_ThrowsException()
        {
            var userProduct = new UserProductItemDto
            {
                UserName = "John",
                ProductName = "Plan1",
                Cost = -5
            };

            try
            {
                await _service!.UpdateUserProductAsync(userProduct);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(ex.Message, "Cost");
            }
        }

        [TestMethod]
        public async Task GetUserProductsByUserIdAsync_CallsRepositoryOnce_WhenCached()
        {
            // First call → repo called
            var result1 = await _service!.GetUserProductsByUserIdAsync(1);

            // Second call → should hit cache, not repo
            var result2 = await _service!.GetUserProductsByUserIdAsync(1);

            Assert.AreEqual(result1.Count(), result2.Count());

            _mockRepo!.Verify(r => r.GetUserProductsByUserIdAsync(1), Times.Once,
                "Repo should be called only once due to caching.");
        }
    }
}
