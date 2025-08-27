using GoogleDriveUnittestWithDapper.Repositories.SearchRepo;
using GoogleDriveUnittestWithDapper.Services.SearchService;
using Moq;
using static GoogleDriveUnittestWithDapper.Dto.SearchDto;

namespace GoogleDriveUnittestWithDapper.Test
{
    [TestClass]
    public class TestSearch
    {
        private Mock<ISearchRepository>? _mockRepo;
        private ISearchService? _searchService;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<ISearchRepository>();

            _mockRepo.Setup(r => r.SearchFilesAsync(It.Is<SearchQueryDto>(q => q.SearchTerm == "sample1")))
                .ReturnsAsync(new List<SearchResultDto>
                {
                    new SearchResultDto
                    {
                        UserFileName = "Doc1.pdf",
                        OwnerEmail = "john@example.com",
                        Bm25Score = 1.5
                    }
                });

            _mockRepo.Setup(r => r.SearchFilesAsync(It.Is<SearchQueryDto>(q => q.SearchTerm == "nonexistent")))
                .ReturnsAsync(new List<SearchResultDto>());

            _mockRepo.Setup(r => r.SearchFilesAsync(It.Is<SearchQueryDto>(q => q.SearchTerm == "sample" && q.Page == 1)))
                .ReturnsAsync(new List<SearchResultDto>
                {
                    new SearchResultDto { UserFileName = "Doc1.pdf", OwnerEmail = "john@example.com", Bm25Score = 2.0 },
                    new SearchResultDto { UserFileName = "Book1.docx", OwnerEmail = "john@example.com", Bm25Score = 1.2 }
                });

            _mockRepo.Setup(r => r.SearchFilesAsync(It.Is<SearchQueryDto>(q => q.SearchTerm == "sample" && q.Page == 2)))
                .ReturnsAsync(new List<SearchResultDto>
                {
                    new SearchResultDto { UserFileName = "Note1.txt", OwnerEmail = "john@example.com", Bm25Score = 0.9 }
                });

            _mockRepo.Setup(r => r.SearchFilesAsync(It.Is<SearchQueryDto>(q => q.SearchTerm == "sample" && q.UserId == 2)))
                .ReturnsAsync(new List<SearchResultDto>
                {
                    new SearchResultDto { UserFileName = "Doc1.pdf", OwnerEmail = "john@example.com", Bm25Score = 1.7 }
                });

            _searchService = new SearchService(_mockRepo.Object);
        }

        [TestMethod]
        public async Task SearchFilesAsync_ValidTerm_ReturnsMatchingFiles()
        {
            var query = new SearchQueryDto { SearchTerm = "sample1", UserId = 1, Page = 1, PageSize = 10 };

            var results = await _searchService!.SearchFilesAsync(query);

            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count());
            var result = results.First();
            Assert.AreEqual("Doc1.pdf", result.UserFileName);
            Assert.AreEqual("john@example.com", result.OwnerEmail);
            Assert.IsGreaterThan(0, result.Bm25Score);
        }

        [TestMethod]
        public async Task SearchFilesAsync_NonExistentTerm_ReturnsEmpty()
        {
            var query = new SearchQueryDto { SearchTerm = "nonexistent", UserId = 1, Page = 1, PageSize = 10 };

            var results = await _searchService!.SearchFilesAsync(query);

            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        public async Task SearchFilesAsync_Pagination_ReturnsCorrectPage()
        {
            var queryPage1 = new SearchQueryDto { SearchTerm = "sample", UserId = 1, Page = 1, PageSize = 2 };
            var queryPage2 = new SearchQueryDto { SearchTerm = "sample", UserId = 1, Page = 2, PageSize = 2 };

            var page1Results = await _searchService!.SearchFilesAsync(queryPage1);
            var page2Results = await _searchService.SearchFilesAsync(queryPage2);

            Assert.IsNotNull(page1Results);
            Assert.AreEqual(2, page1Results.Count());
            Assert.IsTrue(page1Results.Any(r => r.UserFileName == "Doc1.pdf"));

            Assert.IsNotNull(page2Results);
            Assert.AreEqual(1, page2Results.Count());
            Assert.IsTrue(page2Results.Any(r => r.UserFileName == "Note1.txt"));
        }

        [TestMethod]
        public async Task SearchFilesAsync_UserAccessControl_ReturnsOnlyAccessibleFiles()
        {
            var query = new SearchQueryDto { SearchTerm = "sample", UserId = 2, Page = 1, PageSize = 10 };

            var results = await _searchService!.SearchFilesAsync(query);

            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
            Assert.IsTrue(results.All(r => r.UserFileName == "Doc1.pdf"));
        }
    }
}
