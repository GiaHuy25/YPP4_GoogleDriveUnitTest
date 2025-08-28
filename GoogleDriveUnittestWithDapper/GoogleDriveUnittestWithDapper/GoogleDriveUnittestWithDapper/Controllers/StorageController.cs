using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Services.StorageService;
using Microsoft.AspNetCore.Mvc;

namespace GoogleDriveUnittestWithDapper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StorageController : ControllerBase
    {
        private readonly IStorageService _storageService;

        public StorageController(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetStorageByUserIdAsync(int userId)
        {
            try
            {
                var storage = await _storageService.GetStorageByUserIdAsync(userId);
                if (storage == null || !storage.Any())
                    return NotFound(new { message = $"No storage records found for User ID {userId}" });

                return Ok(storage);
            }
            catch (ArgumentException ex) { return BadRequest(new { error = ex.Message }); }
            catch (Exception ex) { return StatusCode(500, new { error = ex.Message }); }
        }

        [HttpPut("{userId}/used-capacity")]
        public async Task<IActionResult> UpdateUsedCapacityAsync(int userId, [FromQuery] int fileSize)
        {
            try
            {
                var result = await _storageService.UpdateUsedCapacityAsync(userId, fileSize);
                return Ok(new { updated = result });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("add-file")]
        public async Task<IActionResult> AddFileToStorageAsync([FromBody] StorageDto storage)
        {
            try
            {
                var fileId = await _storageService.AddFileToStorageAsync(storage);
                return Ok(new { fileId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
