using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Services.BannedUserService;
using Microsoft.AspNetCore.Mvc;

namespace GoogleDriveUnittestWithDapper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BannedUserController : ControllerBase
    {
        private readonly IBannedUserService _bannedService;

        public BannedUserController(IBannedUserService bannedService)
        {
            _bannedService = bannedService ?? throw new ArgumentNullException(nameof(bannedService));
        }
        [HttpGet("{userId}")]
        public async Task<ActionResult<BannedUserDto>> GetBannedUserByUserId(int userId)
        {
            try
            {
                var user = await _bannedService.GetBannedUserByUserId(userId);
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
