using MVCImplement.Dtos;
using MVCImplement.Services.AuthenService;
using MVCImplement.Services.UserService;
using System.Text.Json;

namespace MVCImplement.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IAuthenService _authenService;

        public UserController(IUserService userService, IAuthenService authenService)
        {
            _userService = userService;
            _authenService = authenService;
        }

        public async Task GetAllUsers(IHttpContextWrapper context)
        {
            var users = _userService.GetAllUsers();
            var jsonResponse = JsonSerializer.Serialize(users);
            await WriteResponse(context.Response, jsonResponse, 200, "application/json");
        }

        public async Task GetUserById(IHttpContextWrapper context, int id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
            {
                await WriteResponse(context.Response, $"User with Id {id} not found", 404, "text/plain");
                return;
            }

            var jsonResponse = JsonSerializer.Serialize(user);
            await WriteResponse(context.Response, jsonResponse, 200, "application/json");
        }

        public async Task AddUser(IHttpContextWrapper context, UserDto userDto)
        {
            if (userDto == null || string.IsNullOrEmpty(userDto.Username) || string.IsNullOrEmpty(userDto.Email))
            {
                await WriteResponse(context.Response, "Invalid user data", 400, "text/plain");
                return;
            }

            _userService.AddUser(userDto);
            await WriteResponse(context.Response, "User added successfully", 201, "text/plain");
        }

        public async Task UpdateUser(IHttpContextWrapper context, UserDto userDto)
        {
            if (userDto == null || userDto.Id <= 0)
            {
                await WriteResponse(context.Response, "Invalid user data", 400, "text/plain");
                return;
            }

            var existing = _userService.GetUserById(userDto.Id);
            if (existing == null)
            {
                await WriteResponse(context.Response, $"User with Id {userDto.Id} not found", 404, "text/plain");
                return;
            }

            _userService.UpdateUser(userDto);
            await WriteResponse(context.Response, "User updated successfully", 200, "text/plain");
        }

        public async Task DeleteUser(IHttpContextWrapper context, int id)
        {
            var existing = _userService.GetUserById(id);
            if (existing == null)
            {
                await WriteResponse(context.Response, $"User with Id {id} not found", 404, "text/plain");
                return;
            }

            _userService.DeleteUser(id);
            await WriteResponse(context.Response, "User deleted successfully", 200, "text/plain");
        }
    }
}
