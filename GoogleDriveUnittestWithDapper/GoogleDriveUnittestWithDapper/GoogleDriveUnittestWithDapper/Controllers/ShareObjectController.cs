using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Services.ShareObjectService;
using Microsoft.AspNetCore.Mvc;

namespace GoogleDriveUnittestWithDapper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShareObjectController : ControllerBase
    {
        private readonly IShareService _shareService;

        public ShareObjectController(IShareService shareService)
        {
            _shareService = shareService;
        }


        [HttpGet("{userId}")]
        public async Task<ActionResult<BannedUserDto>> GetSharedObjectsByUserIdAsync(int userId)
        {
            try
            {
                var user = await _shareService.GetSharedObjectsByUserIdAsync(userId);
                if (user == null)
                    return NotFound(new { message = $"User with ID {userId} not found" });

                return Ok(user);
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
