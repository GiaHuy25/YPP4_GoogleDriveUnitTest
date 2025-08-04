using UnitTestGoogleDriveWithADO.Database.Account;
using UnitTestGoogleDriveWithADO.Services;
using UnitTestGoogleDriveWithADO.Models;

namespace UnitTestGoogleDriveWithADO
{
    [TestClass]
    public class GoogleDriveTest
    {
        private readonly AccountService service = new(new AccountRepository());
        [TestMethod]
        public void TestCreateAccount()
        {
            var account = new Account
            {
                UserName = "testuser",
                Email = "test@example.com",
                PasswordHash = "hash123"
            };
            service.RegisterUser(account);
            var fetched = service.GetAccountByEmail("test@example.com");
            Assert.IsNotNull(fetched);
            Assert.AreEqual("testuser", fetched.UserName);
        }

    }
}
