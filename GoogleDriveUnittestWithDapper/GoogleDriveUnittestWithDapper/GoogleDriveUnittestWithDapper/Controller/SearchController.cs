using GoogleDriveUnittestWithDapper.Repositories.SearchRepo;
using GoogleDriveUnittestWithDapper.Services.SearchService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GoogleDriveUnittestWithDapper.Dto.SearchDto;

namespace GoogleDriveUnittestWithDapper.Controller
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
