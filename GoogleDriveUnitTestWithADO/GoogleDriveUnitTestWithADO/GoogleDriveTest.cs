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
                PasswordHash = "hash123"
            };
            service.RegisterUser(account);
            var fetched = service.GetAccountByEmail("test@example.com");
            Assert.IsNotNull(fetched);
            Assert.AreEqual("testuser", fetched.UserName);
            service.DeleteAccount("test@example.com");
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
