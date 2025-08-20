using static GoogleDriveUnittestWithDapper.Dto.SearchDto;

namespace GoogleDriveUnittestWithDapper.Repositories.SearchRepo
{
    public interface ISearchRepository
    {
        Task<IEnumerable<SearchResultDto>> SearchFilesAsync(SearchQueryDto query);
    }
}
