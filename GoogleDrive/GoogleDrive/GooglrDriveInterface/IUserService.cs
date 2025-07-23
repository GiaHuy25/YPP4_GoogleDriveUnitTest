using GoogleDrive.GoogleDriveModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDrive.GooglrDriveInterface
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(string name, string email, string passwordHash);
        Task<User> GetUserByEmailAsync(string email);
        Task UpdateUserLastLoginAsync(int userId);
        Task DeleteUserAsync(int userId);
    }
}
