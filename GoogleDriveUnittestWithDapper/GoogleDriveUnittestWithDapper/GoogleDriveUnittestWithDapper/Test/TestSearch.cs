using GoogleDriveUnittestWithDapper.Controller;
using System.Data;
using static GoogleDriveUnittestWithDapper.Dto.SearchDto;

namespace GoogleDriveUnittestWithDapper.Test
{
    [TestClass]
    public class TestSearch
    {
        private IDbConnection? _dbConnection;
        private SearchController? _searchController;

        [TestInitialize]
        public void Setup()
        {
            var container = DIConfig.ConfigureServices();
            _dbConnection = container.Resolve<IDbConnection>();
            _dbConnection.Open();
            TestDatabaseSchema.CreateSchema(_dbConnection);
            TestDatabaseSchema.InsertSampleData(_dbConnection);

            _searchController = container.Resolve<SearchController>();
        }
        [TestCleanup]
        public void Cleanup()
        {
            _dbConnection.Close();
            _dbConnection.Dispose();
        }
        [TestMethod]
        public async Task SearchFilesAsync_ValidTerm_ReturnsMatchingFiles()
        {
            // Arrange
            var query = new SearchQueryDto
            {
                SearchTerm = "sample1",
                UserId = 1,
                Page = 1,
                PageSize = 10
            };

            // Act
            var results = await _searchController.SearchFilesAsync(query);

            // Assert
            Assert.IsNotNull(results, "Search results should not be null.");
            Assert.AreEqual(1, results.Count(), "Should return exactly one file for 'sample1'.");
            var result = results.First();
            Assert.AreEqual("Doc1.pdf", result.UserFileName, "File name should match.");
            Assert.AreEqual("john@example.com", result.OwnerEmail, "Owner email should match.");
            Assert.IsGreaterThan(0, result.Bm25Score, "BM25 score should be positive.");
        }

        [TestMethod]
        public async Task SearchFilesAsync_NonExistentTerm_ReturnsEmpty()
        {
            // Arrange
            var query = new SearchQueryDto
            {
                SearchTerm = "nonexistent",
                UserId = 1,
                Page = 1,
                PageSize = 10
            };

            // Act
            var results = await _searchController.SearchFilesAsync(query);

            // Assert
            Assert.IsNotNull(results, "Search results should not be null.");
            Assert.AreEqual(0, results.Count(), "Should return no results for nonexistent term.");
        }

        [TestMethod]
        public async Task SearchFilesAsync_Pagination_ReturnsCorrectPage()
        {
            // Arrange
            var queryPage1 = new SearchQueryDto
            {
                SearchTerm = "sample",
                UserId = 1,
                Page = 1,
                PageSize = 2
            };
            var queryPage2 = new SearchQueryDto
            {
                SearchTerm = "sample",
                UserId = 1,
                Page = 2,
                PageSize = 2
            };

            // Act
            var page1Results = await _searchController.SearchFilesAsync(queryPage1);
            var page2Results = await _searchController.SearchFilesAsync(queryPage2);

            // Assert
            Assert.IsNotNull(page1Results, "Page 1 results should not be null.");
            Assert.IsLessThanOrEqualTo(2, page1Results.Count(), "Page 1 should return at most 2 results.");
            Assert.IsNotNull(page2Results, "Page 2 results should not be null.");
            Assert.IsLessThanOrEqualTo(1, page2Results.Count(), "Page 2 should return at most 1 result.");
            Assert.IsTrue(page1Results.Any(r => r.UserFileName == "Doc1.pdf"), "Page 1 should contain Doc1.pdf.");
            Assert.IsTrue(page2Results.Any(r => r.UserFileName == "Note1.txt"), "Page 2 should contain Note1.txt.");
        }

        [TestMethod]
        public async Task SearchFilesAsync_UserAccessControl_ReturnsOnlyAccessibleFiles()
        {
            // Arrange
            var query = new SearchQueryDto
            {
                SearchTerm = "sample",
                UserId = 2, // Jane, who has access via shares
                Page = 1,
                PageSize = 10
            };

            // Act
            var results = await _searchController.SearchFilesAsync(query);

            // Assert
            Assert.IsNotNull(results, "Search results should not be null.");
            Assert.IsTrue(results.Any(), "Jane should have access to shared files.");
            Assert.IsTrue(results.All(r => r.UserFileName == "Doc1.pdf"), "Jane should only see shared file Doc1.pdf.");
        }
    }
}
