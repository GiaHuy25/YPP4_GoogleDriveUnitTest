using static GoogleDriveUnittestWithDapper.Dto.SearchDto;

namespace GoogleDriveUnittestWithDapper.Services.SearchService
{
    public interface ISearchService
    {
        Task<IEnumerable<SearchResultDto>> SearchFilesAsync(SearchQueryDto query);
    }
}
