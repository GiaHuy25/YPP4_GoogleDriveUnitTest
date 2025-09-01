using GoogleDriveUnittestWithDapper.Repositories.AccountRepo;
using GoogleDriveUnittestWithDapper.Repositories.BannedUserRepo;
using GoogleDriveUnittestWithDapper.Repositories.SearchRepo;
using GoogleDriveUnittestWithDapper.Repositories.ShareObjectRepo;
using GoogleDriveUnittestWithDapper.Repositories.StorageRepo;
using GoogleDriveUnittestWithDapper.Repositories.TrashRepo;
using GoogleDriveUnittestWithDapper.Repositories.UserFileFolderRepo;
using GoogleDriveUnittestWithDapper.Services.AccountService;
using GoogleDriveUnittestWithDapper.Services.BannedUserService;
using GoogleDriveUnittestWithDapper.Services.SearchService;
using GoogleDriveUnittestWithDapper.Services.ShareObjectService;
using GoogleDriveUnittestWithDapper.Services.StorageService;
using GoogleDriveUnittestWithDapper.Services.TrashService;
using GoogleDriveUnittestWithDapper.Services.UserFileFolderService;
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
            builder.Services.AddScoped<IBannedUserRepository, BannedUserRepository>();
            builder.Services.AddScoped<ISearchRepository, SearchRepository>();
            builder.Services.AddScoped<IShareRepository, ShareRepository>();
            builder.Services.AddScoped<IStorageRepository, StorageRepository>();
            builder.Services.AddScoped<ITrashRepository, TrashRepository>();
            builder.Services.AddScoped<IUserFileFolderRepository, UserFileFolderRepository>();

            // Service
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IBannedUserService, BannedUserService>();
            builder.Services.AddScoped<ISearchService, SearchService>();
            builder.Services.AddScoped<IShareService,ShareService>();
            builder.Services.AddScoped<IStorageService, StorageService>();
            builder.Services.AddScoped<ITrashService, TrashService>();
            builder.Services.AddScoped<IUserFileFolderService, UserFileFolderService>();

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
