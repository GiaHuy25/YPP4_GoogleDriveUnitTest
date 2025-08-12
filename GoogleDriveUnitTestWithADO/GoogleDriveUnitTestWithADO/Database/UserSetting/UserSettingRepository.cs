using Microsoft.Data.SqlClient;
using GoogleDriveUnitTestWithADO.Models;

namespace GoogleDriveUnitTestWithADO.Database.UserSettingRepo
{
    public class UserSettingRepository : IUserSettingRepository
    {
        public int AddUserSetting(UserSetting userSetting)
        {
            if (userSetting.BooleanValue.HasValue == userSetting.AppSettingOptionId.HasValue)
            {
                throw new ArgumentException("Exactly one of BooleanValue or AppSettingOptionId must be set.");
            }

            using var conn = DataAccess.DatabaseHelper.GetConnection();
            conn.Open();
            string query = @"INSERT INTO UserSetting (UserId, AppSettingKeyId, BooleanValue, AppSettingOptionId)
                            VALUES (@UserId, @AppSettingKeyId, @BooleanValue, @AppSettingOptionId);
                            SELECT SCOPE_IDENTITY();";

            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@UserId", userSetting.UserId);
            cmd.Parameters.AddWithValue("@AppSettingKeyId", userSetting.AppSettingKeyId);
            cmd.Parameters.AddWithValue("@BooleanValue", (object)userSetting.BooleanValue ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@AppSettingOptionId", (object)userSetting.AppSettingOptionId ?? DBNull.Value);

            decimal id = (decimal)cmd.ExecuteScalar();
            return (int)id;
        }

        public UserSetting GetUserSettingById(int id)
        {
            using var conn = DataAccess.DatabaseHelper.GetConnection();
            conn.Open();
            string query = "SELECT * FROM UserSetting WHERE Id = @Id";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new UserSetting
                {
                    id = (int)reader["Id"],
                    UserId = (int)reader["UserId"],
                    AppSettingKeyId = (int)reader["AppSettingKeyId"],
                    BooleanValue = reader["BooleanValue"] as bool?,
                    AppSettingOptionId = reader["AppSettingOptionId"] as int?
                };
            }
            return null;
        }

        public void UpdateUserSetting(UserSetting userSetting)
        {
            if (userSetting.BooleanValue.HasValue == userSetting.AppSettingOptionId.HasValue)
            {
                throw new ArgumentException("Exactly one of BooleanValue or AppSettingOptionId must be set.");
            }

            using var conn = DataAccess.DatabaseHelper.GetConnection();
            conn.Open();
            string query = @"UPDATE UserSetting 
                            SET UserId = @UserId,
                                AppSettingKeyId = @AppSettingKeyId,
                                BooleanValue = @BooleanValue,
                                AppSettingOptionId = @AppSettingOptionId
                            WHERE Id = @Id";

            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@Id", userSetting.id);
            cmd.Parameters.AddWithValue("@UserId", userSetting.UserId);
            cmd.Parameters.AddWithValue("@AppSettingKeyId", userSetting.AppSettingKeyId);
            cmd.Parameters.AddWithValue("@BooleanValue", (object)userSetting.BooleanValue ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@AppSettingOptionId", (object)userSetting.AppSettingOptionId ?? DBNull.Value);

            cmd.ExecuteNonQuery();
        }

        public void DeleteUserSetting(int id)
        {
            using var conn = DataAccess.DatabaseHelper.GetConnection();
            conn.Open();
            string query = "DELETE FROM UserSetting WHERE Id = @Id";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
