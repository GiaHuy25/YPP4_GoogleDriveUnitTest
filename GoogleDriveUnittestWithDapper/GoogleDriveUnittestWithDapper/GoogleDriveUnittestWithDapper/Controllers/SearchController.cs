using GoogleDriveUnittestWithDapper.Services.SearchService;
using static GoogleDriveUnittestWithDapper.Dto.SearchDto;

namespace GoogleDriveUnittestWithDapper.Controllers
{
    public class SearchController
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        public async Task<IEnumerable<SearchResultDto>> SearchFilesAsync(SearchQueryDto query)
        {
            return await _searchService.SearchFilesAsync(query);
        }
    }
}
