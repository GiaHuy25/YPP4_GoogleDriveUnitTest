using MVCImplement.Services.AuthenService;
using System.Text.Json;

namespace MVCImplement.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IAuthenService _authService;

        public AuthController(IAuthenService authService)
        {
            _authService = authService;
        }

        public async Task Login(IHttpContextWrapper context, string username, string password)
        {
            if (!_authService.Authenticate(username, password))
            {
                var error = JsonSerializer.Serialize(new { error = "Unauthorized" });
                await WriteResponse(context.Response, error, 401);
                return;
            }

            await WriteResponse(context.Response, "{\"message\":\"Login successful\"}", 200);
        }
    }
}
