using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Services.AccountService;
using Microsoft.AspNetCore.Mvc;

namespace GoogleDriveUnittestWithDapper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        }

        // GET: api/account/5
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserByIdAsync(int userId)
        {
            try
            {
                var user = await _accountService.GetUserByIdAsync(userId);
                if (user == null)
                    return NotFound(new { message = $"User with ID {userId} not found" });

                return Ok(user);
            }
            catch (ArgumentException ex) { return BadRequest(new { error = ex.Message }); }
            catch (Exception ex) { return StatusCode(500, new { error = ex.Message }); }
        }

        // POST: api/account
        [HttpPost]
        public async Task<IActionResult> AddUserAsync([FromBody] CreateAccountDto accountDto)
        {
            try
            {
                var created = await _accountService.AddUserAsync(accountDto);
                return CreatedAtAction(nameof(GetUserByIdAsync),
                                       new { userId = created.UserId },
                                       created);
            }
            catch (ArgumentException ex) { return BadRequest(new { error = ex.Message }); }
            catch (Exception ex) { return StatusCode(500, new { error = ex.Message }); }
        }

        // DELETE: api/account/5
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUserAsync(int userId)
        {
            try
            {
                var result = await _accountService.DeleteUserAsync(userId);
                if (!result)
                    return NotFound(new { message = $"User with ID {userId} not found" });

                return NoContent();
            }
            catch (ArgumentException ex) { return BadRequest(new { error = ex.Message }); }
            catch (Exception ex) { return StatusCode(500, new { error = ex.Message }); }
        }

        // PUT: api/account
        [HttpPut]
        public async Task<IActionResult> UpdateUserAsync([FromBody] AccountDto accountDto)
        {
            try
            {
                var updated = await _accountService.UpdateUserAsync(accountDto);
                if (updated == null)
                    return NotFound(new { message = $"User with ID {accountDto.UserId} not found" });

                return Ok(updated);
            }
            catch (ArgumentException ex) { return BadRequest(new { error = ex.Message }); }
            catch (Exception ex) { return StatusCode(500, new { error = ex.Message }); }
        }
    }
}
