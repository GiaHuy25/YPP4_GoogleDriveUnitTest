using Microsoft.Data.SqlClient;

namespace GoogleDriveUnitTestWithADO.Database.Account
{
    public class AccountRepository : IAccountRepository
    {
        public void Add(Models.Account acc)
        {
            using var conn = DataAccess.DatabaseHelper.GetConnection();
            conn.Open();
            var cmd = new SqlCommand("INSERT INTO Account (UserName, Email, PasswordHash, CreatedAt, UserImg, LastLogin, UsedCapacity, Capacity) VALUES (@UserName, @Email, @PasswordHash, @CreatedAt, @UserImg, @LastLogin, @UsedCapacity, @Capacity)", conn);
            cmd.Parameters.AddWithValue("@UserName", acc.UserName);
            cmd.Parameters.AddWithValue("@Email", acc.Email);
            cmd.Parameters.AddWithValue("@PasswordHash", acc.PasswordHash);
            cmd.Parameters.AddWithValue("@CreatedAt", acc.CreatedAt);
            cmd.Parameters.AddWithValue("@UserImg", acc.UserImg);
            cmd.Parameters.AddWithValue("@LastLogin", (object)acc.LastLogin ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UsedCapacity", (object)acc.UsedCapacity ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Capacity", (object)acc.Capacity ?? DBNull.Value);
            cmd.ExecuteNonQuery();
        }
        public Models.Account GetByEmail(string email)
        {
            using var conn = DataAccess.DatabaseHelper.GetConnection();
            conn.Open();
            var cmd = new SqlCommand("SELECT * FROM Account WHERE Email = @Email", conn);
            cmd.Parameters.AddWithValue("@Email", email);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Models.Account
                {
                    UserId = (int)reader["UserId"],
                    UserName = reader["UserName"].ToString(),
                    Email = reader["Email"].ToString(),
                    PasswordHash = reader["PasswordHash"].ToString(),
                    CreatedAt = (DateTime)reader["CreatedAt"],
                    UserImg = reader["UserImg"].ToString(),
                    LastLogin = reader["LastLogin"] as DateTime?,
                    UsedCapacity = reader["UsedCapacity"] as long?,
                    Capacity = reader["Capacity"] as long?
                };
            }
            return null;
        }
        public void Update(Models.Account acc)
        {
            using var conn = DataAccess.DatabaseHelper.GetConnection();
            conn.Open();
            var cmd = new SqlCommand("UPDATE Account SET UserName = @UserName, Email = @Email, PasswordHash = @PasswordHash, CreatedAt = @CreatedAt, UserImg = @UserImg, LastLogin = @LastLogin, UsedCapacity = @UsedCapacity, Capacity = @Capacity WHERE UserId = @UserId", conn);
            cmd.Parameters.AddWithValue("@UserId", acc.UserId);
            cmd.Parameters.AddWithValue("@UserName", acc.UserName);
            cmd.Parameters.AddWithValue("@Email", acc.Email);
            cmd.Parameters.AddWithValue("@PasswordHash", acc.PasswordHash);
            cmd.Parameters.AddWithValue("@CreatedAt", acc.CreatedAt);
            cmd.Parameters.AddWithValue("@UserImg", acc.UserImg);
            cmd.Parameters.AddWithValue("@LastLogin", (object)acc.LastLogin ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UsedCapacity", (object)acc.UsedCapacity ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Capacity", (object)acc.Capacity ?? DBNull.Value);
            cmd.ExecuteNonQuery();
        }
        public void Delete(string email)
        {
            using var conn = DataAccess.DatabaseHelper.GetConnection();
            conn.Open();
            var cmd = new SqlCommand("DELETE FROM Account WHERE Email = @Email", conn);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.ExecuteNonQuery();
        }
        public List<Models.Account> GetAll()
        {
            var accounts = new List<Models.Account>();
            using var conn = DataAccess.DatabaseHelper.GetConnection();
            conn.Open();
            var cmd = new SqlCommand("SELECT * FROM Account", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                accounts.Add(new Models.Account
                {
                    UserId = (int)reader["UserId"],
                    UserName = reader["UserName"].ToString(),
                    Email = reader["Email"].ToString(),
                    PasswordHash = reader["PasswordHash"].ToString(),
                    CreatedAt = (DateTime)reader["CreatedAt"],
                    UserImg = reader["UserImg"].ToString(),
                    LastLogin = reader["LastLogin"] as DateTime?,
                    UsedCapacity = reader["UsedCapacity"] as long?,
                    Capacity = reader["Capacity"] as long?
                });
            }
            return accounts;
        }
    }
}
