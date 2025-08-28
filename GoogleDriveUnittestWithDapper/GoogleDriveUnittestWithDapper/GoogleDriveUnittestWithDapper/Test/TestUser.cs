using GoogleDriveUnittestWithDapper.Controllers;
using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.AccountRepo;
using GoogleDriveUnittestWithDapper.Services.AccountService;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GoogleDriveUnittestWithDapper.Test
{
    [TestClass]
    public class TestAccount
    {
        private Mock<IAccountService>? _mockService;
        private AccountController? _controller;

        private Mock<IAccountRepository>? _mockRepo;
        private AccountService? _service;

        [TestInitialize]
        public void Setup()
        {
            // --- Mock Service cho Controller test ---
            _mockService = new Mock<IAccountService>();
            _controller = new AccountController(_mockService.Object);

            _mockService.Setup(s => s.GetUserByIdAsync(1))
                .ReturnsAsync(new AccountDto { UserId = 1, UserName = "John" });
            _mockService.Setup(s => s.GetUserByIdAsync(999))
                .ReturnsAsync((AccountDto?)null);

            // --- Mock Repository cho Service test ---
            _mockRepo = new Mock<IAccountRepository>();
            _service = new AccountService(_mockRepo.Object);

            _mockRepo.Setup(r => r.GetUserByIdAsync(1))
                .ReturnsAsync(new AccountDto { UserId = 1, UserName = "RepoJohn" });
            _mockRepo.Setup(r => r.GetAllUsersAsync())
                .ReturnsAsync(new List<AccountDto>
                {
                    new AccountDto { UserId = 1, UserName = "RepoJohn" },
                    new AccountDto { UserId = 2, UserName = "RepoAlice" }
                });
        }

        // ---------- TEST CONTROLLER ----------
        [TestMethod]
        public async Task Controller_GetUserById_Valid_ReturnsOk()
        {
            var result = await _controller!.GetUserByIdAsync(1) as OkObjectResult;

            Assert.IsNotNull(result);
            var user = result.Value as AccountDto;
            Assert.AreEqual("John", user!.UserName);
        }

        [TestMethod]
        public async Task Controller_GetUserById_Invalid_ReturnsNotFound()
        {
            var result = await _controller!.GetUserByIdAsync(999);
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        // ---------- TEST SERVICE ----------
        [TestMethod]
        public async Task Service_GetUserById_Valid_ReturnsUser()
        {
            var user = await _service!.GetUserByIdAsync(1);

            Assert.IsNotNull(user);
            Assert.AreEqual("RepoJohn", user.UserName);
        }

        [TestMethod]
        public async Task Service_GetAllUsers_ReturnsList()
        {
            var users = await _service!.GetAllUsersAsync();

            Assert.AreEqual(2, users.Count());
            Assert.IsTrue(users.Any(u => u.UserName == "RepoAlice"));
        }
    }
}
