using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Services.UserProductService;
using Microsoft.AspNetCore.Mvc;

namespace GoogleDriveUnittestWithDapper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProductController : ControllerBase
    {
        private readonly IUserProductService _userProductService;

        public UserProductController(IUserProductService userProductService)
        {
            _userProductService = userProductService;
        }

        // GET: api/UserProduct/{userId}
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<UserProductItemDto>>> GetUserProductsByUserIdAsync(int userId)
        {
            try
            {
                var products = await _userProductService.GetUserProductsByUserIdAsync(userId);
                return Ok(products);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST: api/UserProduct
        [HttpPost]
        public async Task<ActionResult<int>> AddUserProductAsync([FromBody] UserProductItemDto userProduct)
        {
            if (userProduct == null)
                return BadRequest("UserProduct data is required.");

            try
            {
                var id = await _userProductService.AddUserProductAsync(userProduct);
                return Ok(id);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/UserProduct
        [HttpPut]
        public async Task<ActionResult<int>> UpdateUserProductAsync([FromBody] UserProductItemDto userProduct)
        {
            if (userProduct == null)
                return BadRequest("UserProduct data is required.");

            try
            {
                var result = await _userProductService.UpdateUserProductAsync(userProduct);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
