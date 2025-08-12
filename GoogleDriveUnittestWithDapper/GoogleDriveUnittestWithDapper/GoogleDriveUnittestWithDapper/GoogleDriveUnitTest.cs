using Dapper;
using GoogleDriveUnittestWithDapper.Repositories.FolderRepo;
using GoogleDriveUnittestWithDapper.Services.FolderService;
using Microsoft.Data.Sqlite;
using System.Data;

namespace GoogleDriveUnittestWithDapper
{
    [TestClass]
    public class GoogleDriveUnitTest
    {
        private IDbConnection _connection;
        private IFolderService _folderService;

        [TestInitialize]
        public void Setup()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
            _connection.Execute("PRAGMA foreign_keys = ON;");

            _connection.Execute(@"
                CREATE TABLE Account (
                    UserId INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserName TEXT NOT NULL,
                    Email TEXT UNIQUE NOT NULL,
                    PasswordHash TEXT NOT NULL,
                    CreatedAt TEXT NOT NULL DEFAULT (datetime('now'))
                );

                CREATE TABLE Folder (
                    FolderId INTEGER PRIMARY KEY AUTOINCREMENT,
                    ParentId INTEGER,
                    OwnerId INTEGER NOT NULL,
                    FolderName TEXT NOT NULL,
                    CreatedAt TEXT NOT NULL DEFAULT (datetime('now')),
                    UpdatedAt TEXT,
                    FolderPath TEXT,
                    FolderStatus TEXT,
                    ColorId INTEGER,
                    FOREIGN KEY (ParentId) REFERENCES Folder(FolderId),
                    FOREIGN KEY (OwnerId) REFERENCES Account(UserId)
                );
            ");

            _connection.Execute("INSERT INTO Account (UserName, Email, PasswordHash) VALUES ('John', 'john@example.com', 'hash123');");

            var repo = new FolderRepository(_connection);
            _folderService = new FolderService(repo);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _connection.Dispose();
        }

        [TestMethod]
        public void CanCreateAndRetrieveFolder()
        {
            var folder = _folderService.CreateFolder("TestFolder", 1);
            var fetched = _folderService.GetFolderById(folder.FolderId);

            Assert.IsNotNull(fetched);
            Assert.AreEqual("TestFolder", fetched.FolderName);
        }
    }
}