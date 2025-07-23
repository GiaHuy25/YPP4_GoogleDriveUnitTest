using GoogleDrive.GoogleDriveModel;
using GoogleDrive.GooglrDriveInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDrive.GoogleDriveService
{
    public class UserService : IUserService
    {
        public async Task<User> CreateUserAsync(string name, string email, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(passwordHash))
            {
                throw new ArgumentException("Name, email, and password hash are required.");
            }
            if (name.Length > 100 || email.Length > 255) // Schema constraints: NVARCHAR(100), NVARCHAR(255)
            {
                throw new ArgumentException("Name or email exceeds maximum length.");
            }
            return await Task.FromResult(new User
            {
                UserId = 1,
                Name = name,
                Email = email,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow
            });
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email is required.");
            }
            if (email == "test@example.com")
            {
                return await Task.FromResult(new User
                {
                    UserId = 1,
                    Name = "Test User",
                    Email = email,
                    PasswordHash = "hashed_password",
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                });
            }
            return await Task.FromResult((User)null);
        }

        public async Task UpdateUserLastLoginAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid user ID.");
            }
            if (userId != 1)
            {
                throw new ArgumentException("User not found.");
            }
            await Task.CompletedTask;
        }

        public async Task DeleteUserAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid user ID.");
            }
            if (userId != 1)
            {
                throw new ArgumentException("User not found.");
            }
            await Task.CompletedTask;
        }
    }
}
