namespace MVCImplement.Services.UserService
{
    public class UserService : IUserService
    {
        private static readonly UserService _instance = new UserService();

        private UserService() { }

        public static UserService Instance => _instance;

        public string GetUserInfo(string username)
        {
            return $"User: {username}";
        }
    }
}
