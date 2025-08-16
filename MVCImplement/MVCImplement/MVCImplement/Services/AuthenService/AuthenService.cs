namespace MVCImplement.Services.AuthenService
{
    public class AuthenService : IAuthenService
    {
        private static readonly AuthenService _instance = new AuthenService();

        public AuthenService() { }

        public static AuthenService Instance => _instance;

        public bool Authenticate(string username, string password)
        {
            return username == "user" && password == "pass"; // Mock auth
        }
    }
}
