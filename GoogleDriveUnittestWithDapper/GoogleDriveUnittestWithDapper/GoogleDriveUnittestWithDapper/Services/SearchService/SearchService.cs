using GoogleDriveUnittestWithDapper.Repositories.SearchRepo;
using static GoogleDriveUnittestWithDapper.Dto.SearchDto;

namespace GoogleDriveUnittestWithDapper.Services.SearchService
{
    public class SearchService : ISearchService
    {
        private readonly ISearchRepository _searchRepository;

        public SearchService(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }

        public async Task<IEnumerable<SearchResultDto>> SearchFilesAsync(SearchQueryDto query)
        {
            if (string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                throw new ArgumentException("Search term cannot be empty.");
            }

            if (query.Page < 1)
            {
                query.Page = 1;
            }

            if (query.PageSize < 1)
            {
                query.PageSize = 10;
            }

            return await _searchRepository.SearchFilesAsync(query);
        }
    }
}
