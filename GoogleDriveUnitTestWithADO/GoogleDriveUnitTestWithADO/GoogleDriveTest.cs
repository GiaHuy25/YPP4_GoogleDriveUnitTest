using GoogleDriveUnitTestWithADO.Database.Account;
using GoogleDriveUnitTestWithADO.Database.Folder;
using GoogleDriveUnitTestWithADO.Services;
using GoogleDriveUnitTestWithADO.Models;


namespace GoogleDriveUnitTestWithADO
{
    [TestClass]
    public class GoogleDriveTest
    {
        private readonly AccountService service = new(new AccountRepository());
        private readonly FolderService folderService = new(new FolderRepository());
        [TestMethod]
        public void TestCreateAccount()
        {
            var account = new Account
            {
                UserName = "testuser",
                Email = "test@example.com",
                PasswordHash = "hash123",
                UsedCapacity = 62235696,
                Capacity = 200000000
            };
            service.RegisterUser(account);
            var fetched = service.GetAccountByEmail("test@example.com");
            Assert.IsNotNull(fetched);
            Assert.AreEqual("testuser", fetched.UserName);
        }
        [TestMethod]
        public void TestGetAccountByEmail() {
            var account = service.GetAccountByEmail("aaron85@thompson.com");
            Assert.IsNotNull(account);
            Assert.AreEqual("castroabigail", account.UserName);
            Assert.AreEqual(62235696, account.UsedCapacity);
            Assert.AreEqual(200000000, account.Capacity);
        }
        [TestMethod]
        public void TestUpdateAccount()
        {
            var email = "aaron85@thompson.com";

            var account = service.GetAccountByEmail(email);
            Assert.IsNotNull(account, "Account should exist before updating.");

            var originalUserName = account.UserName;
            account.UserName = "updatedUserName";
            account.UsedCapacity = 75000000; 
            account.LastLogin = DateTime.Now;

            service.updateAccount(account);

            var updatedAccount = service.GetAccountByEmail(email);
            Assert.IsNotNull(updatedAccount, "Updated account should exist.");
            Assert.AreEqual("updatedUserName", updatedAccount.UserName, "UserName should be updated.");
            Assert.AreEqual(75000000, updatedAccount.UsedCapacity, "UsedCapacity should be updated.");
            Assert.IsNotNull(updatedAccount.LastLogin, "LastLogin should be updated.");
            Assert.AreEqual(account.Capacity, updatedAccount.Capacity, "Capacity should remain unchanged.");
        }
        [TestMethod]
        public void TestDeleteAccount() 
        {
            var email = "test@example.com";
            var account = service.GetAccountByEmail(email);
            Assert.IsNotNull(account, "Account should exist before deletion.");
            service.DeleteAccount(email);
            var deletedAccount = service.GetAccountByEmail(email);
            Assert.IsNull(deletedAccount, "Account should be deleted.");
        }
        [TestMethod]
        public void TestCreateFolder()
        {
            var folder = new Folder
            {
                FolderName = "UnitTestFolder",
                ParentId = null,
                OwnerId = 1,
                CreatedAt = DateTime.Now
            };

            folderService.AddFolder(folder);
            var fetched = folderService.GetFolderById(1);

            Assert.IsNotNull(fetched);
            Assert.AreEqual("UnitTestFolder", fetched.FolderName);
        }
    }

}
