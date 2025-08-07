using DatabaseFunction.Services;
using DatabaseFunction.Models;
namespace DatabaseFunction
{
    [TestClass]
    public class DatabaseFunctionTest
    {
        private readonly SqlService<Account, Folder> _accountFolderService = new SqlService<Account, Folder>();
        private readonly SqlService<Account, UserFile> _accountFileService = new SqlService<Account, UserFile>();
        private readonly SqlService<Folder, UserFile> _folderFileService = new SqlService<Folder, UserFile>();
        private List<Account> _accounts;
        private List<Folder> _folders;
        private List<UserFile> _files;
        [TestInitialize]
        public void Setup()
        {
            _accounts = new List<Account>
        {
            new Account { UserId = 1, UserName = "Alice", Email = "alice@example.com", CreatedAt = DateTime.Now, UsedCapacity = 1000, Capacity = 10000 },
            new Account { UserId = 2, UserName = "Bob", Email = "bob@example.com", CreatedAt = DateTime.Now, UsedCapacity = 2000, Capacity = 20000 },
            new Account { UserId = 3, UserName = "Charlie", Email = "charlie@example.com", CreatedAt = DateTime.Now, UsedCapacity = 3000, Capacity = 30000 }
        };

            _folders = new List<Folder>
        {
            new Folder { FolderId = 1, OwnerId = 1, FolderName = "AliceDocs", CreatedAt = DateTime.Now },
            new Folder { FolderId = 2, OwnerId = 2, FolderName = "BobDocs", CreatedAt = DateTime.Now },
            new Folder { FolderId = 3, OwnerId = 3, FolderName = "CharlieDocs", CreatedAt = DateTime.Now }
        };

            _files = new List<UserFile>
        {
            new UserFile { FileId = 1, FolderId = 1, OwnerId = 1, UserFileName = "Doc1.txt", Size = 100, CreatedAt = DateTime.Now },
            new UserFile { FileId = 2, FolderId = 1, OwnerId = 1, UserFileName = "Doc2.txt", Size = 200, CreatedAt = DateTime.Now },
            new UserFile { FileId = 3, FolderId = 2, OwnerId = 2, UserFileName = "Doc3.txt", Size = 150, CreatedAt = DateTime.Now }
        };
        }
        [TestMethod]
        public void CrossJoin_returnAll()
        { 
            var result = _accountFolderService.CrossJoin(_accounts.AsEnumerable(), _folders.AsEnumerable());
            Assert.IsNotNull(result);
            Assert.AreEqual(9, result.Count);
            Assert.IsTrue(result.Any(r => r.Item1.UserName == "Alice" && r.Item2.FolderName == "AliceDocs"));
            Assert.IsTrue(result.Any(r => r.Item1.UserName == "Bob" && r.Item2.FolderName == "BobDocs"));
        }
        [TestMethod]
        public void LeftJoin_returnMatchingAndNonMatching()
        {
            var result = _accountFolderService.LeftJoin(_accounts.AsEnumerable(), _folders.AsEnumerable(), (a,f) => a.UserId == f.OwnerId);
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            Assert.IsTrue(result.Any(r => r.Item1.UserName == "Alice" && r.Item2.FolderName == "AliceDocs"));
            Assert.IsTrue(result.Any(r => r.Item1.UserName == "Bob" && r.Item2.FolderName == "BobDocs"));
            Assert.IsTrue(result.Any(r => r.Item1.UserName == "Charlie" && r.Item2.FolderName == "CharlieDocs"));
        }
        [TestMethod]
        public void Where_returnFiltered()
        {
            var result = _accountFolderService.Where(_accounts.AsEnumerable(), a => a.UsedCapacity > 1500);
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(a => a.UserName == "Bob"));
            Assert.IsTrue(result.Any(a => a.UserName == "Charlie"));
        }
        [TestMethod]
        public void Aggregate_returnSum()
        {
            var result = _accountFolderService.Aggregate(_accounts.AsEnumerable(), a => a.UsedCapacity ?? 0, "sum");
            Assert.AreEqual(6000, result);
        }
        [TestMethod]
        public void LeftJoin_ReturnAccountFile()
        {
            var result = _accountFileService.LeftJoin(_accounts, _files, FunctionAbc());
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count);
            Assert.IsTrue(result.Any(r => r.Item1.UserName == "Alice" && r.Item2.UserFileName == "Doc1.txt"));
            Assert.IsTrue(result.Any(r => r.Item1.UserName == "Alice" && r.Item2.UserFileName == "Doc2.txt"));
            Assert.IsTrue(result.Any(r => r.Item1.UserName == "Bob" && r.Item2.UserFileName == "Doc3.txt"));
            Assert.IsTrue(result.Any(r => r.Item1.UserName == "Charlie" && r.Item2 == null));

           
        }
        static Func<Account, UserFile, bool> FunctionAbc()
        {
            return (l, r) => l.UserId == r.OwnerId;
        }
        [TestMethod]
        public void Aggregate_returnAvg()
        {
            var result = _accountFolderService.Aggregate(_accounts.AsEnumerable(), a => a.UsedCapacity ?? 0, "avg");
            Assert.AreEqual(2000, result);
        }
        [TestMethod]
        public void Aggregate_returnMax()
        {
            var result = _accountFolderService.Aggregate(_accounts.AsEnumerable(), a => a.UsedCapacity ?? 0, "max");
            Assert.AreEqual(3000, result);
        }
        [TestMethod]
        public void Aggregate_returnMin()
        {
            var result = _accountFolderService.Aggregate(_accounts.AsEnumerable(), a => a.UsedCapacity ?? 0, "min");
            Assert.AreEqual(1000, result);
        }
        [TestMethod]
        public void aggregate_returnCount()
        {
            var result = _accountFolderService.Aggregate(_accounts.AsEnumerable(), a => a.UserId, "count");
            Assert.AreEqual(3, result);
        }
        [TestMethod]
        public void GetFileWithOwner()
        {
            int Owner = 1;
            var ResultUser = _accountFileService.Where(_accounts.AsEnumerable(), a => a.UserId == Owner);
            var ResultFiles = _accountFileService.LeftJoin(ResultUser.AsEnumerable(), _files.AsEnumerable(), (a, f) => a.UserId == f.OwnerId );
            Assert.IsNotNull(ResultFiles);
            Assert.AreEqual(2, ResultFiles.Count);
            Assert.IsTrue(ResultFiles.Any(r => r.Item1.UserName == "Alice" && r.Item2.UserFileName == "Doc1.txt"));
            Assert.IsTrue(ResultFiles.Any(r => r.Item1.UserName == "Alice" && r.Item2.UserFileName == "Doc2.txt"));

        }
        [TestMethod]
        public void GetFileOfFolder()
        {
            int Folder = 1;
            var ResultFolder = _folderFileService.Where(_folders.AsEnumerable(), f => f.FolderId == Folder);
            var ResultFiles = _folderFileService.LeftJoin(ResultFolder.AsEnumerable(), _files.AsEnumerable(), (f, file) => f.FolderId == file.FolderId);
            Assert.IsNotNull(ResultFiles);
            Assert.AreEqual(2, ResultFiles.Count);
            Assert.IsTrue(ResultFiles.Any(r => r.Item1.FolderName == "AliceDocs" && r.Item2.UserFileName == "Doc1.txt"));
            Assert.IsTrue(ResultFiles.Any(r => r.Item1.FolderName == "AliceDocs" && r.Item2.UserFileName == "Doc2.txt"));
        }
    }
}