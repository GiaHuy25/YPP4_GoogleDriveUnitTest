using Microsoft.Data.SqlClient;
using GoogleDriveUnitTestWithADO.Models;

namespace GoogleDriveUnitTestWithADO.Database.PermissionRepo
{
    public class PermissionRepository : IPermissionRepositoy
    {
        private static readonly Dictionary<int, string> PermissionMap = new Dictionary<int, string>
        {
            {1,"reader" },
            {2,"Contributor" },
            {3,"Owner" }
        };

        public int AddPermission(Permission permission)
        {
            if (!PermissionMap.ContainsValue(permission.PermissionName))
            {
                throw new ArgumentException("Invalid permission name.");
            }
            using var conn = DataAccess.DatabaseHelper.GetConnection();
            conn.Open();
            string query = "INSERT INTO Permission (PermissionName, PermissionPriority) " +
                            "VALUES (@PermissionName, @PermissionPriority); " +
                            "SELECT SCOPE_IDENTITY();";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@PermissionName", permission.PermissionName);
            cmd.Parameters.AddWithValue("@PermissionPriority", permission.PermissionPriority);
            decimal permissionId = (decimal)cmd.ExecuteScalar();
            return (int)permissionId;
        }

        public void DeletePermission(int permissionId)
        {
            using var conn = DataAccess.DatabaseHelper.GetConnection();
            conn.Open();
            string query = "DELETE FROM Permission WHERE PermissionId = @PermissionId";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@PermissionId", permissionId);
            cmd.ExecuteNonQuery();
        }

        public Permission GetPermissionById(int permissionId)
        {
            using var conn = DataAccess.DatabaseHelper.GetConnection();
            conn.Open();
            string query = "SELECT * FROM Permission WHERE PermissionId = @PermissionId";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@PermissionId", permissionId);
            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Permission
                {
                    PermissionId = (int)reader["PermissionId"],
                    PermissionName = reader["PermissionName"] as string,
                    PermissionPriority = (int)reader["PermissionPriority"]
                };
            }
            return null;
        }

        public string GetPermissionNameById(int permissionId)
        {
            using var conn = DataAccess.DatabaseHelper.GetConnection();
            conn.Open();
            string query = "SELECT PermissionName FROM Permission WHERE PermissionId = @PermissionId";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@PermissionId", permissionId);
            var result = cmd.ExecuteScalar();
            return result?.ToString() ?? string.Empty;
        }

        public void UpdatePermission(Permission permission)
        {
            if (!PermissionMap.ContainsValue(permission.PermissionName))
            {
                throw new ArgumentException("Invalid permission name.");
            }
            using var conn = DataAccess.DatabaseHelper.GetConnection();
            conn.Open();
            string query = "UPDATE Permissions SET " +
                           "PermissionName = @PermissionName, " +
                           "PermissionPriority = @PermissionPriority " +
                           "WHERE PermissionId = @PermissionId";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@PermissionId", permission.PermissionId);
            cmd.Parameters.AddWithValue("@PermissionName", permission.PermissionName);
            cmd.Parameters.AddWithValue("@PermissionPriority", permission.PermissionPriority);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
