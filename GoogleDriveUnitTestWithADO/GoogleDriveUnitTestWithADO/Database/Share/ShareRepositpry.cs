using Microsoft.Data.SqlClient;
using GoogleDriveUnitTestWithADO.Models;

namespace GoogleDriveUnitTestWithADO.Database.ShareRepo
{
    public class ShareRepositpry : IShareRepository
    {
        public int AddShare(Share share)
        {
            using var conn = DataAccess.DatabaseHelper.GetConnection();
            conn.Open();
            string query = "INSERT INTO Share (Sharer, ObjectId, ObjectTypeId, CreatedAt, ShareUrl, UrlApprove) " +
                "VALUES (@Sharer, @ObjectId, @ObjectTypeId, @CreatedAt, @ShareUrl, @UrlApprove); " +
                "SELECT SCOPE_IDENTITY();";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@Sharer", share.Sharer);
            cmd.Parameters.AddWithValue("@ObjectId", share.ObjectId);
            cmd.Parameters.AddWithValue("@ObjectTypeId", share.ObjectTypeId);
            cmd.Parameters.AddWithValue("@CreatedAt", share.CreatedAt);
            cmd.Parameters.AddWithValue("@ShareUrl", share.ShareUrl);
            cmd.Parameters.AddWithValue("@UrlApprove", share.UrlApprove);
            decimal shareId = (decimal)cmd.ExecuteScalar();
            return (int)shareId;
        }

        public void DeleteShare(int shareId)
        {
            using var conn = DataAccess.DatabaseHelper.GetConnection();
            conn.Open();
            string query = "DELETE FROM Share WHERE ShareId = @ShareId";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@ShareId", shareId);
            cmd.ExecuteNonQuery();
        }

        public Share GetShareById(int shareId)
        {
            using var conn = DataAccess.DatabaseHelper.GetConnection();
            conn.Open();
            string query = "SELECT * FROM Share WHERE ShareId = @ShareId";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@ShareId", shareId);
            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Share
                {
                    ShareId = (int)reader["ShareId"],
                    Sharer = (int)reader["Sharer"],
                    ObjectId = (int)reader["ObjectId"],
                    ObjectTypeId = (int)reader["ObjectTypeId"],
                    CreatedAt = (DateTime)reader["CreatedAt"],
                    ShareUrl = reader["ShareUrl"] as string,
                    UrlApprove = (bool)reader["UrlApprove"]
                };
            }
            return null;
        }

        public void UpdateShare(Share share)
        {
            using var conn = DataAccess.DatabaseHelper.GetConnection();
            conn.Open();
            string query = "UPDATE Share " +
                            "SET Sharer = @Sharer, " +
                                "ObjectId = @ObjectId, " +
                                "ObjectTypeId = @ObjectTypeId, " +
                                "CreatedAt = @CreatedAt, " +
                                "ShareUrl = @ShareUrl, " +
                                "UrlApprove = @UrlApprove " +
                            "WHERE ShareId = @ShareId";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@ShareId", share.ShareId);
            cmd.Parameters.AddWithValue("@Sharer", share.Sharer);
            cmd.Parameters.AddWithValue("@ObjectId", share.ObjectId);
            cmd.Parameters.AddWithValue("@ObjectTypeId", share.ObjectTypeId);
            cmd.Parameters.AddWithValue("@CreatedAt", share.CreatedAt);
            cmd.Parameters.AddWithValue("@ShareUrl", share.ShareUrl ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@UrlApprove", share.UrlApprove);
            cmd.ExecuteNonQuery();
        }
    }
}
