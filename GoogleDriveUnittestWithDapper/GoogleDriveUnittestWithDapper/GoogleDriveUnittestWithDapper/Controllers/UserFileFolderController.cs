using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Services.UserFileFolderService;
using Microsoft.AspNetCore.Mvc;

namespace GoogleDriveUnittestWithDapper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserFileFolderController : ControllerBase
    {
        private readonly IUserFileFolderService _userFileAndFolderService;

        public UserFileFolderController(IUserFileFolderService userFileAndFolderService)
        {
            _userFileAndFolderService = userFileAndFolderService;
        }

        // GET: api/UserFileFolder/files-folders/{userId}
        [HttpGet("files-folders/{userId}")]
        public ActionResult<IEnumerable<UserFileAndFolderDto>> GetFilesAndFoldersByUserId(int userId)
        {
            var result = _userFileAndFolderService.GetFilesAndFoldersByUserId(userId)?.ToList();
            if (result == null || !result.Any())
                return NotFound($"No files or folders found for userId {userId}.");

            return Ok(result);
        }

        // GET: api/UserFileFolder/files/{userId}
        [HttpGet("files/{userId}")]
        public ActionResult<IEnumerable<FileDto>> GetFilesByUserId(int userId)
        {
            var result = _userFileAndFolderService.GetFilesByUserId(userId)?.ToList();
            if (result == null || !result.Any())
                return NotFound($"No files found for userId {userId}.");

            return Ok(result);
        }

        // GET: api/UserFileFolder/folder/{folderId}
        [HttpGet("folder/{folderId}")]
        public ActionResult<IEnumerable<FolderDto>> GetFolderById(int folderId)
        {
            var result = _userFileAndFolderService.GetFolderById(folderId)?.ToList();
            if (result == null || !result.Any())
                return NotFound($"No folder found with id {folderId}.");

            return Ok(result);
        }

        // GET: api/UserFileFolder/favorites/{userId}
        [HttpGet("favorites/{userId}")]
        public ActionResult<IEnumerable<FavoriteObjectOfUserDto>> GetFavoritesByUserId(int userId)
        {
            var result = _userFileAndFolderService.GetFavoritesByUserId(userId)?.ToList();
            if (result == null || !result.Any())
                return NotFound($"No favorites found for userId {userId}.");

            return Ok(result);
        }
    }
}
