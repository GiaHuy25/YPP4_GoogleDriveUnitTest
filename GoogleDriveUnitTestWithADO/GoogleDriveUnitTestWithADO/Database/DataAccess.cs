using Microsoft.Data.SqlClient;

namespace GoogleDriveUnitTestWithADO.Database
{
    public static class DataAccess
    {
        public static class DatabaseHelper
        {
            private const string ConnectionString = "Server=INTERN-HUYPHAN\\MSSQLSERVER03;Database=GoogleDrive;Trusted_Connection=True;Encrypt=False;";

            public static SqlConnection GetConnection()
            {
                return new SqlConnection(ConnectionString);
            }
        }
    }
}
