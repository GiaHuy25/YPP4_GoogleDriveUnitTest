using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GoogleDriveUnittestWithDapper.Dto.SearchDto;

namespace GoogleDriveUnittestWithDapper.Services.SearchService
{
    public interface ISearchService
    {
        Task<IEnumerable<SearchResultDto>> SearchFilesAsync(SearchQueryDto query);
    }
}
