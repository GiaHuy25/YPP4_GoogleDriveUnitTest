using GoogleDriveUnittestWithDapper.Services.FolderService;
using Microsoft.Data.Sqlite;
using System.Data;
using GoogleDriveUnittestWithDapper.Repositories.BannedUserRepo;
using GoogleDriveUnittestWithDapper.Services.BannedUserService;

namespace GoogleDriveUnittestWithDapper
{
    [TestClass]
    public class GoogleDriveUnitTest
    {
        private IDbConnection _connection;
        private IFolderService _folderService;
        private IBannedUserRepository _bannedUserRepository;
        private IBannedUserService _bannedUserService;

        [TestInitialize]
        public void Setup()
        {
            // Use in-memory SQLite database
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();

            // Create schema and insert sample data
            TestDatabaseSchema.CreateSchema(_connection);
            TestDatabaseSchema.InsertSampleData(_connection);

            _bannedUserRepository = new BannedUserRepository(_connection);
            _bannedUserService = new BannedUserService(_bannedUserRepository);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _connection.Dispose();
        }
    }
}