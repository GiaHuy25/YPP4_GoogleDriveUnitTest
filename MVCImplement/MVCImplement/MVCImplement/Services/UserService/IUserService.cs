using MVCImplement.Dtos;

namespace MVCImplement.Services.UserService
{
    public interface IUserService
    {
        IEnumerable<UserDto> GetAllUsers();
        UserDto? GetUserById(int id);
        void AddUser(UserDto userDto);
        void UpdateUser(UserDto userDto);
        void DeleteUser(int id);
    }
}
