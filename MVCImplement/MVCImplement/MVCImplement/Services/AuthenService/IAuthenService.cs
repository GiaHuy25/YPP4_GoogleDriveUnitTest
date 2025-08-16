namespace MVCImplement.Services.AuthenService
{
    public interface IAuthenService
    {
        bool Authenticate(string username, string password);
    }
}
