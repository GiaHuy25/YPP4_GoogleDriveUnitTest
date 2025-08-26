using GoogleDriveUnittestWithDapper.Repositories.AccountRepo;
using GoogleDriveUnittestWithDapper.Services.AccountService;
using Microsoft.Data.Sqlite;
using System.Data;

namespace GoogleDriveUnittestWithDapper
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connection = new SqliteConnection(TestDatabaseSchema.ConnectionString);
            connection.Open();

            TestDatabaseSchema.CreateSchema(connection);
            TestDatabaseSchema.InsertSampleData(connection);

            // Register IDbConnection (reuse the same opened connection)
            builder.Services.AddSingleton<IDbConnection>(connection);

            // Repository
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();

            // Service
            builder.Services.AddScoped<IAccountService, AccountService>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
