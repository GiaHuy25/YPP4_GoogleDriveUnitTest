
using Microsoft.Data.SqlClient;

namespace GoogleDriveUnitTestWithADO.Database.SharedUser
{
    public class SharedUserRepository : ISharedUserRepository
    {
        public int addSharedUser(Models.SharedUser sharedUser)
        {
            using var conn = DataAccess.DatabaseHelper.GetConnection();
            conn.Open();
            string query = @"INSERT INTO SharedUser (ShareId, UserId, PermissionId, CreatedAt, ModifiedAt)
                            VALUES (@ShareId, @UserId, @PermissionId, @CreatedAt, @ModifiedAt);
                            SELECT SCOPE_IDENTITY();";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@ShareId", sharedUser.ShareId);
            cmd.Parameters.AddWithValue("@UserId", sharedUser.UserId);
            cmd.Parameters.AddWithValue("@PermissionId", sharedUser.PermissionId);
            cmd.Parameters.AddWithValue("@CreatedAt", sharedUser.CreatedAt);
            cmd.Parameters.AddWithValue("@ModifiedAt", (object?)sharedUser.ModifiedAt ?? DBNull.Value);
            decimal sharedUserId = (decimal)cmd.ExecuteScalar();
            return (int)sharedUserId;
        }

        public void DeleteSharedUser(int sharedUserId)
        {
            using var conn = DataAccess.DatabaseHelper.GetConnection();
            conn.Open();
            string query = "DELETE FROM SharedUser WHERE SharedUserId = @SharedUserId";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@SharedUserId", sharedUserId);
            cmd.ExecuteNonQuery();
        }

        public Models.SharedUser GetSharedUserById(int sharedUserId)
        {
           using var conn = DataAccess.DatabaseHelper.GetConnection();
            conn.Open();
            string query = "SELECT * FROM SharedUser WHERE SharedUserId = @SharedUserId";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@SharedUserId", sharedUserId);
            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Models.SharedUser
                {
                    SharedUserId = (int)reader["SharedUserId"],
                    ShareId = (int)reader["ShareId"],
                    UserId = (int)reader["UserId"],
                    PermissionId = (int)reader["PermissionId"],
                    CreatedAt = (DateTime)reader["CreatedAt"],
                    ModifiedAt = reader["ModifiedAt"] as DateTime?
                };
            }
            return null;
        }

        public void UpdateSharedUser(Models.SharedUser sharedUser)
        {
           using var conn = DataAccess.DatabaseHelper.GetConnection();
            conn.Open();
            string query = "UPDATE SharedUser SET " +
                                "ShareId = @ShareId," +
                                " UserId = @UserId, " +
                                "PermissionId = @PermissionId, " +
                                "CreatedAt = @CreatedAt, " +
                                "ModifiedAt = @ModifiedAt " +
                            "WHERE SharedUserId = @SharedUserId";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@SharedUserId", sharedUser.SharedUserId);
            cmd.Parameters.AddWithValue("@ShareId", sharedUser.ShareId);
            cmd.Parameters.AddWithValue("@UserId", sharedUser.UserId);
            cmd.Parameters.AddWithValue("@PermissionId", sharedUser.PermissionId);
            cmd.Parameters.AddWithValue("@CreatedAt", sharedUser.CreatedAt);
            cmd.Parameters.AddWithValue("@ModifiedAt", (object?)sharedUser.ModifiedAt ?? DBNull.Value);
            cmd.ExecuteNonQuery();
        }
    }
}
