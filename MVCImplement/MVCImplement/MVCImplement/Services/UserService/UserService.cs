using MVCImplement.Dtos;
using MVCImplement.Models;
using MVCImplement.Repositories;

namespace MVCImplement.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly Repository<Users> _repository;

        public UserService()
        {
            _repository = new Repository<Users>();
        }

        public IEnumerable<UserDto> GetAllUsers()
        {
            return _repository.GetAll()
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    FullName = u.FullName,
                    CreatedAt = u.CreatedAt
                });
        }

        public UserDto? GetUserById(int id)
        {
            var user = _repository.GetById(id).FirstOrDefault();
            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName,
                CreatedAt = user.CreatedAt
            };
        }

        public void AddUser(UserDto userDto)
        {
            var user = new Users
            {
                Username = userDto.Username,
                Email = userDto.Email,
                FullName = userDto.FullName,
                Password = userDto.Password,
                CreatedAt = DateTime.UtcNow
            };
            _repository.Add(user);
        }

        public void UpdateUser(UserDto userDto)
        {
            var existingUser = _repository.GetById(userDto.Id).FirstOrDefault();
            if (existingUser == null) return;

            var user = new Users
            {
                Id = userDto.Id,
                Username = userDto.Username,
                Email = userDto.Email,
                FullName = userDto.FullName,
                Password = userDto.Password,
                CreatedAt = existingUser.CreatedAt 
            };
            _repository.Update(user);
        }


        public void DeleteUser(int id)
        {
            _repository.Delete(id);
        }
    }
}
