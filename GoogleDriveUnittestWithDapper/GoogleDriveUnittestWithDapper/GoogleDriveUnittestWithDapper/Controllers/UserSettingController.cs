using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Services.UserSettingService;
using Microsoft.AspNetCore.Mvc;

namespace GoogleDriveUnittestWithDapper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserSettingController : ControllerBase
    {
        private readonly IUserSettingService _userSettingService;

        public UserSettingController(IUserSettingService userSettingService)
        {
            _userSettingService = userSettingService;
        }

        // GET: api/UserSetting/{userId}
        [HttpGet("{userId}")]
        public ActionResult<IEnumerable<UserSettingDto>> GetUserSettings(int userId)
        {
            try
            {
                var settings = _userSettingService.GetUserSettings(userId);

                if (settings == null || !settings.Any())
                {
                    return NotFound($"No settings found for UserId = {userId}");
                }

                return Ok(settings);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error while retrieving user settings: {ex.Message}");
            }
        }
    }
}
