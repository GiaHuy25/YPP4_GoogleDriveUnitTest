using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestGoogleDriveWithADO.Database
{
    public static class DataAccess
    {
        public static class DatabaseHelper {
            private const string ConnectionString = "Server=INTERN-HUYPHAN\\MSSQLSERVER03;Database=GoogleDrive;Trusted_Connection=True;Encrypt=False;";

            public static SqlConnection GetConnection()
            {
                return new SqlConnection(ConnectionString);
            }
        }
    }
}
