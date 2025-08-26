using GoogleDriveUnittestWithDapper.Controllers;
using GoogleDriveUnittestWithDapper.Repositories.AccountRepo;
using GoogleDriveUnittestWithDapper.Repositories.BannedUserRepo;
using GoogleDriveUnittestWithDapper.Repositories.SearchRepo;
using GoogleDriveUnittestWithDapper.Repositories.ShareObjectRepo;
using GoogleDriveUnittestWithDapper.Repositories.StorageRepo;
using GoogleDriveUnittestWithDapper.Repositories.TrashRepo;
using GoogleDriveUnittestWithDapper.Repositories.UserFileFolderRepo;
using GoogleDriveUnittestWithDapper.Repositories.UserProductRepo;
using GoogleDriveUnittestWithDapper.Repositories.UserSettingRepo;
using GoogleDriveUnittestWithDapper.Services.AccountService;
using GoogleDriveUnittestWithDapper.Services.BannedUserService;
using GoogleDriveUnittestWithDapper.Services.SearchService;
using GoogleDriveUnittestWithDapper.Services.ShareObjectService;
using GoogleDriveUnittestWithDapper.Services.StorageService;
using GoogleDriveUnittestWithDapper.Services.TrashService;
using GoogleDriveUnittestWithDapper.Services.UserFileFolderService;
using GoogleDriveUnittestWithDapper.Services.UserProductService;
using GoogleDriveUnittestWithDapper.Services.UserSettingService;
using Microsoft.Data.Sqlite;
using System.Data;

namespace GoogleDriveUnittestWithDapper
{
    public class DIConfig
    {
        public static SimpleContainer ConfigureServices()
        {
            var container = new SimpleContainer();
            container.Register<IDbConnection, SqliteConnection>(Lifetime.Singleton);
            container.RegisterFactory(() => new SqliteConnection("Data Source=:memory:"), Lifetime.Singleton);


            container.Register<IAccountRepository, AccountRepository>(Lifetime.Transient);
            container.Register<IAccountService, AccountService>(Lifetime.Transient);
            container.Register<AccountController>(Lifetime.Transient);

            container.Register<IBannedUserRepository, BannedUserRepository>(Lifetime.Transient);
            container.Register<IBannedUserService, BannedUserService>(Lifetime.Transient);
            container.Register<BannedUserController>(Lifetime.Transient);

            container.Register<IShareRepository, ShareRepository>(Lifetime.Transient);
            container.Register<IShareService, ShareService>(Lifetime.Transient);
            container.Register<ShareObjectController>(Lifetime.Transient);

            container.Register<IStorageRepository, StorageRepository>(Lifetime.Transient);
            container.Register<IStorageService, StorageService>(Lifetime.Transient);
            container.Register<StorageController>(Lifetime.Transient);

            container.Register<ITrashRepository, TrashRepository>(Lifetime.Transient);
            container.Register<ITrashService, TrashService>(Lifetime.Transient);
            container.Register<TrashController>(Lifetime.Transient);

            container.Register<IUserFileFolderRepository, UserFileFolderRepository>(Lifetime.Transient);
            container.Register<IUserFileFolderService, UserFileFolderService>(Lifetime.Transient);
            container.Register<UserFileFolderController>(Lifetime.Transient);

            container.Register<IUserProductRepository, UserProductRepository>(Lifetime.Transient);
            container.Register<IUserProductService, UserProductService>(Lifetime.Transient);
            container.Register<UserProductController>(Lifetime.Transient);

            container.Register<IUserSettingRepository, UserSettingRepository>(Lifetime.Transient);
            container.Register<IUserSettingService, UserSettingService>(Lifetime.Transient);
            container.Register<UserSettingController>(Lifetime.Transient);

            container.Register<ISearchRepository, SearchRepository>(Lifetime.Transient);
            container.Register<ISearchService, SearchService>(Lifetime.Transient);
            container.Register<SearchController>(Lifetime.Transient);

            return container;
        }
    }
}
