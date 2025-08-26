using MVCImplement.Services.AuthenService;
using MVCImplement.Services.UserService;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task GetUserInfo(IHttpContextWrapper context)
        {
            var items = context.GetType().GetProperty("Items")?.GetValue(context) as NameValueCollection;
            var username = items?["username"] ?? "Unknown";
            var userInfo = _userService.GetUserInfo(username);

            await WriteResponse(context.Response, userInfo, 200, "text/html");
        }
    }
}
