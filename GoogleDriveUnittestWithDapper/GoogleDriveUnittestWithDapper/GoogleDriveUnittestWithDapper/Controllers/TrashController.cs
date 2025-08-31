using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Services.TrashService;
using Microsoft.AspNetCore.Mvc;

namespace GoogleDriveUnittestWithDapper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrashController : ControllerBase
    {
        private readonly ITrashService _trashService;

        public TrashController(ITrashService trashService)
        {
            _trashService = trashService;
        }

        // POST: api/trash/add
        [HttpPost("add")]
        public async Task<ActionResult<int>> AddToTrashAsync([FromBody] TrashDto trash)
        {
            if (trash == null)
                return BadRequest("Trash data is required.");

            var result = await _trashService.AddToTrashAsync(trash);
            return Ok(result);
        }

        // DELETE: api/trash/clear/{userId}
        [HttpDelete("clear/{userId}")]
        public async Task<ActionResult<int>> ClearTrashAsync(int userId)
        {
            var result = await _trashService.ClearTrashAsync(userId);
            return Ok(result);
        }

        // GET: api/trash/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<TrashDto>>> GetTrashByUserIdAsync(int userId)
        {
            var trashList = await _trashService.GetTrashByUserIdAsync(userId);
            return Ok(trashList);
        }

        // GET: api/trash/{trashId}
        [HttpGet("{trashId}")]
        public async Task<ActionResult<IEnumerable<TrashDto>>> GetTrashByIdAsync(int trashId)
        {
            var trash = await _trashService.GetTrashByIdAsync(trashId);
            return Ok(trash);
        }

        // PUT: api/trash/delete/{trashId}/user/{userId}
        [HttpPut("delete/{trashId}/user/{userId}")]
        public async Task<ActionResult<int>> PermanentlyDeleteFromTrashAsync(int trashId, int userId)
        {
            var result = await _trashService.PermanentlyDeleteFromTrashAsync(trashId, userId);
            return Ok(result);
        }

        // PUT: api/trash/restore/{trashId}/user/{userId}
        [HttpPut("restore/{trashId}/user/{userId}")]
        public async Task<ActionResult<int>> RestoreFromTrashAsync(int trashId, int userId)
        {
            var result = await _trashService.RestoreFromTrashAsync(trashId, userId);
            return Ok(result);
        }
    }
}
