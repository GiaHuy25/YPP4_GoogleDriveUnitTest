using Dapper;
using GoogleDriveUnittestWithDapper.Controller;
using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.FavoriteObjectRepo;
using GoogleDriveUnittestWithDapper.Services.FavoriteObjectService;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Test
{
    [TestClass]
    public class TestFavoriteObject
    {
        private SqliteConnection _connection;
        private IFavoriteObjectRepository _favoriteRepository;
        private IFavoriteObjectService _favoriteService;
        private FavoriteObjectController _favoriteController;

        [TestInitialize]
        public void Setup()
        {
            // Use in-memory SQLite database
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();

            // Create schema and insert sample data
            TestDatabaseSchema.CreateSchema(_connection);
            TestDatabaseSchema.InsertSampleData(_connection);

            // Initialize repository and service
            _favoriteRepository = new FavoriteObjectRepository(_connection);
            _favoriteService = new FavoriteObjectService(_favoriteRepository);
            _favoriteController = new FavoriteObjectController(_favoriteService);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _connection.Close();
            _connection.Dispose();
        }
        [TestMethod]
        public void GetFavoritesByUserId_ValidUserId_ReturnsAllFavorites()
        {
            // Arrange
            int userId = 1;
            // Act
            var result = _favoriteController.GetFavoritesByUserId(userId).ToList();

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.HasCount(1, result, "Number of items does not match");
            Assert.AreEqual("RootFolder", result[0].FolderName);
        }
    }
}
