using GoogleDriveUnittestWithDapper.Services.SearchService;
using Microsoft.AspNetCore.Mvc;
using static GoogleDriveUnittestWithDapper.Dto.SearchDto;

namespace GoogleDriveUnittestWithDapper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpPost("files")]
        public async Task<ActionResult<IEnumerable<SearchResultDto>>> SearchFiles([FromBody] SearchQueryDto query)
        {
            try
            {
                var results = await _searchService.SearchFilesAsync(query);

                if (results == null || !results.Any())
                {
                    return NotFound(new { message = "No results found." });
                }

                return Ok(results);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
