using Dapper;
using System.Data;

namespace GoogleDriveUnittestWithDapper
{
    public static class TestDatabaseSchema
    {
        public static void CreateSchema(IDbConnection connection)
        {
            connection.Execute("PRAGMA foreign_keys = ON;");

            // Full schema adapted for SQLite:
            var sql = @"
                        CREATE TABLE Account (
                            UserId INTEGER PRIMARY KEY AUTOINCREMENT,
                            UserName TEXT NOT NULL,
                            Email TEXT UNIQUE NOT NULL,
                            PasswordHash TEXT NOT NULL,
                            CreatedAt TEXT NOT NULL DEFAULT (datetime('now')),
                            UserImg TEXT,
                            LastLogin TEXT,
                            UsedCapacity INTEGER,
                            Capacity INTEGER
                        );

                        CREATE TABLE Color(
                            ColorId INTEGER PRIMARY KEY AUTOINCREMENT,
                            ColorName TEXT,
                            ColorIcon TEXT
                        );

                        CREATE TABLE Permission (
                            PermissionId INTEGER PRIMARY KEY AUTOINCREMENT,
                            PermissionName TEXT NOT NULL,
                            PermissionPriority INTEGER
                        );

                        CREATE TABLE ObjectType (
                            ObjectTypeId INTEGER PRIMARY KEY AUTOINCREMENT,
                            ObjectTypeName TEXT NOT NULL
                        );

                        CREATE TABLE Folder (
                            FolderId INTEGER PRIMARY KEY AUTOINCREMENT,
                            ParentId INTEGER,
                            OwnerId INTEGER NOT NULL,
                            FolderName TEXT NOT NULL,
                            CreatedAt TEXT NOT NULL DEFAULT (datetime('now')),
                            UpdatedAt TEXT,
                            FolderPath TEXT,
                            FolderStatus TEXT,
                            ColorId INTEGER,
                            FOREIGN KEY (ParentId) REFERENCES Folder(FolderId),
                            FOREIGN KEY (OwnerId) REFERENCES Account(UserId),
                            FOREIGN KEY (ColorId) REFERENCES Color(ColorId)
                        );

                        CREATE TABLE FileType (
                            FileTypeId INTEGER PRIMARY KEY AUTOINCREMENT,
                            FileTypeName TEXT NOT NULL,
                            Icon TEXT
                        );

                        CREATE TABLE UserFile (
                            FileId INTEGER PRIMARY KEY AUTOINCREMENT,
                            FolderId INTEGER,
                            OwnerId INTEGER NOT NULL,
                            Size INTEGER,
                            UserFileName TEXT NOT NULL,
                            UserFilePath TEXT,
                            UserFileThumbNailImg TEXT,
                            FileTypeId INTEGER,
                            ModifiedDate TEXT,
                            UserFileStatus TEXT,
                            CreatedAt TEXT NOT NULL DEFAULT (datetime('now')),
                            FOREIGN KEY (FolderId) REFERENCES Folder(FolderId),
                            FOREIGN KEY (OwnerId) REFERENCES Account(UserId),
                            FOREIGN KEY (FileTypeId) REFERENCES FileType(FileTypeId)
                        );

                        CREATE TABLE Share (
                            ShareId INTEGER PRIMARY KEY AUTOINCREMENT,
                            Sharer INTEGER NOT NULL,
                            ObjectId INTEGER NOT NULL,
                            ObjectTypeId INTEGER NOT NULL,
                            CreatedAt TEXT NOT NULL DEFAULT (datetime('now')),
                            ShareUrl TEXT,
                            UrlApprove INTEGER,
                            FOREIGN KEY (Sharer) REFERENCES Account(UserId),
                            FOREIGN KEY (ObjectTypeId) REFERENCES ObjectType(ObjectTypeId)
                        );

                        CREATE TABLE SharedUser (
                            SharedUserId INTEGER PRIMARY KEY AUTOINCREMENT,
                            ShareId INTEGER,
                            UserId INTEGER,
                            PermissionId INTEGER,
                            CreatedAt TEXT,
                            ModifiedAt TEXT,
                            FOREIGN KEY (ShareId) REFERENCES Share(ShareId),
                            FOREIGN KEY (UserId) REFERENCES Account(UserId),
                            FOREIGN KEY (PermissionId) REFERENCES Permission(PermissionId)
                        );

                        CREATE TABLE FileVersion (
                            FileVersionId INTEGER PRIMARY KEY AUTOINCREMENT,
                            FileId INTEGER,
                            FileVersion INTEGER NOT NULL,
                            FileVersionPath TEXT,
                            CreatedAt TEXT,
                            UpdateBy INTEGER,
                            IsCurrent INTEGER,
                            VersionFile TEXT,
                            Size INTEGER,
                            FOREIGN KEY (FileId) REFERENCES UserFile(FileId),
                            FOREIGN KEY (UpdateBy) REFERENCES Account(UserId)
                        );

                        CREATE TABLE Trash (
                            TrashId INTEGER PRIMARY KEY AUTOINCREMENT,
                            ObjectId INTEGER NOT NULL,
                            ObjectTypeId INTEGER NOT NULL,
                            RemovedDatetime TEXT,
                            UserId INTEGER,
                            IsPermanent INTEGER DEFAULT 0,
                            FOREIGN KEY (UserId) REFERENCES Account(UserId),
                            FOREIGN KEY (ObjectTypeId) REFERENCES ObjectType(ObjectTypeId)
                        );

                        CREATE TABLE ProductItem (
                            ProductId INTEGER PRIMARY KEY AUTOINCREMENT,
                            ProductName TEXT NOT NULL,
                            Cost REAL NOT NULL,
                            Duration INTEGER NOT NULL
                        );

                        CREATE TABLE Promotion (
                            PromotionId INTEGER PRIMARY KEY AUTOINCREMENT,
                            PromotionName TEXT NOT NULL,
                            Discount INTEGER NOT NULL,
                            IsPercent INTEGER NOT NULL
                        );

                        CREATE TABLE UserProduct (
                            UserProductId INTEGER PRIMARY KEY AUTOINCREMENT,
                            UserId INTEGER,
                            ProductId INTEGER,
                            PayingDatetime TEXT,
                            IsFirstPaying INTEGER,
                            PromotionId INTEGER,
                            EndDatetime TEXT,
                            FOREIGN KEY (UserId) REFERENCES Account(UserId),
                            FOREIGN KEY (ProductId) REFERENCES ProductItem(ProductId),
                            FOREIGN KEY (PromotionId) REFERENCES Promotion(PromotionId)
                        );

                        CREATE TABLE BannedUser (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            UserId INTEGER,
                            BannedAt TEXT,
                            BannedUserId INTEGER,
                            FOREIGN KEY (UserId) REFERENCES Account(UserId),
                            FOREIGN KEY (BannedUserId) REFERENCES Account(UserId)
                        );

                        CREATE TABLE FavoriteObject (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            OwnerId INTEGER,
                            ObjectId INTEGER NOT NULL,
                            ObjectTypeId INTEGER NOT NULL,
                            FOREIGN KEY (OwnerId) REFERENCES Account(UserId),
                            FOREIGN KEY (ObjectTypeId) REFERENCES ObjectType(ObjectTypeId)
                        );

                        CREATE TABLE ActionRecent (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            UserId INTEGER,
                            ObjectId INTEGER,
                            ObjectTypeId INTEGER,
                            ActionLog TEXT,
                            ActionDateTime TEXT,
                            FOREIGN KEY (UserId) REFERENCES Account(UserId),
                            FOREIGN KEY (ObjectTypeId) REFERENCES ObjectType(ObjectTypeId)
                        );

                        CREATE TABLE SearchHistory (
                            SearchId INTEGER PRIMARY KEY AUTOINCREMENT,
                            UserId INTEGER,
                            SearchToken TEXT,
                            SearchDatetime TEXT,
                            FOREIGN KEY (UserId) REFERENCES Account(UserId)
                        );

                        CREATE TABLE UserSession (
                            SessionId INTEGER PRIMARY KEY AUTOINCREMENT,
                            UserId INTEGER,
                            Token TEXT NOT NULL,
                            CreatedAt TEXT NOT NULL DEFAULT (datetime('now')),
                            ExpiresAt TEXT,
                            FOREIGN KEY (UserId) REFERENCES Account(UserId)
                        );

                        CREATE TABLE AppSettingKey(
                            SettingId INTEGER PRIMARY KEY AUTOINCREMENT,
                            SettingKey TEXT,
                            IsBoolean INTEGER,
                            Decription TEXT
                        );

                        CREATE TABLE AppSettingOption (
                            AppSettingOptionId INTEGER PRIMARY KEY AUTOINCREMENT,
                            SettingKeyId INTEGER NOT NULL,
                            SettingValue TEXT NOT NULL,
                            FOREIGN KEY (SettingKeyId) REFERENCES AppSettingKey(SettingId)
                        );

                        CREATE TABLE UserSetting (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            UserId INTEGER NOT NULL,
                            AppSettingKeyId INTEGER NOT NULL,
                            BooleanValue INTEGER NULL,
                            AppSettingOptionId INTEGER NULL,
                            FOREIGN KEY (AppSettingKeyId) REFERENCES AppSettingKey(SettingId),
                            FOREIGN KEY (AppSettingOptionId) REFERENCES AppSettingOption(AppSettingOptionId),
                            FOREIGN KEY (UserId) REFERENCES Account(UserId)
                        );

                        CREATE INDEX idx_file_name ON UserFile(UserFileName);
                        CREATE INDEX idx_folder_name ON Folder(FolderName);

                        CREATE TABLE FileContent (
                            ContentId INTEGER PRIMARY KEY AUTOINCREMENT,
                            FileId INTEGER NOT NULL,
                            ContentChunk TEXT NOT NULL,
                            ChunkIndex INTEGER NOT NULL,
                            DocumentLength INTEGER NULL,
                            CreatedAt TEXT DEFAULT (datetime('now')),
                            FOREIGN KEY (FileId) REFERENCES UserFile(FileId),
                            UNIQUE (FileId, ChunkIndex)
                        );

                        CREATE TABLE Term (
                            TermId INTEGER PRIMARY KEY AUTOINCREMENT,
                            Term TEXT NOT NULL,
                            FileContentId INTEGER NOT NULL,
                            CreatedAt TEXT DEFAULT (datetime('now')),
                            FOREIGN KEY (FileContentId) REFERENCES FileContent(ContentId)
                        );

                        CREATE TABLE SearchIndex (
                            SearchIndexId INTEGER PRIMARY KEY AUTOINCREMENT,
                            FileContentId INTEGER NOT NULL,
                            Term TEXT NOT NULL,
                            TermFrequency INTEGER NOT NULL,
                            TermPositions TEXT,
                            Bm25Score REAL NOT NULL DEFAULT 0,
                            IDF REAL NOT NULL DEFAULT 0,
                            FOREIGN KEY (FileContentId) REFERENCES FileContent(ContentId),
                            UNIQUE (FileContentId, Term),
                            CHECK (IDF >= 0)
                        );
                        ";
            connection.Execute(sql);
        }
        public static void InsertSampleData(IDbConnection connection)
        {
            // Insert into Account (3 rows)
            connection.Execute("INSERT INTO Account (UserName, Email, PasswordHash, UserImg) VALUES ('John', 'john@example.com', 'hash123', 'img1.jpg');");
            connection.Execute("INSERT INTO Account (UserName, Email, PasswordHash) VALUES ('Jane', 'jane@example.com', 'hash456');");
            connection.Execute("INSERT INTO Account (UserName, Email, PasswordHash) VALUES ('Bob', 'bob@example.com', 'hash789');");

            // Insert into Color (3 rows)
            connection.Execute("INSERT INTO Color (ColorName, ColorIcon) VALUES ('Red', 'red_icon.png');");
            connection.Execute("INSERT INTO Color (ColorName, ColorIcon) VALUES ('Blue', 'blue_icon.png');");
            connection.Execute("INSERT INTO Color (ColorName, ColorIcon) VALUES ('Green', 'green_icon.png');");

            // Insert into Permission (3 rows)
            connection.Execute("INSERT INTO Permission (PermissionName, PermissionPriority) VALUES ('Owner', 1);");
            connection.Execute("INSERT INTO Permission (PermissionName, PermissionPriority) VALUES ('Contributor', 2);");
            connection.Execute("INSERT INTO Permission (PermissionName, PermissionPriority) VALUES ('Viewer', 3);");

            // Insert into ObjectType (3 rows)
            connection.Execute("INSERT INTO ObjectType (ObjectTypeName) VALUES ('Folder');");
            connection.Execute("INSERT INTO ObjectType (ObjectTypeName) VALUES ('File');");
            connection.Execute("INSERT INTO ObjectType (ObjectTypeName) VALUES ('Share');");

            // Insert into Folder (3 rows with hierarchical paths)
            int ownerId1 = connection.QuerySingle<int>("SELECT UserId FROM Account WHERE UserName = 'John'");
            int colorId1 = connection.QuerySingle<int>("SELECT ColorId FROM Color WHERE ColorName = 'Red'");
            connection.Execute("INSERT INTO Folder (ParentId, OwnerId, FolderName, FolderPath, FolderStatus, ColorId) VALUES (NULL, " + ownerId1 + ", 'RootFolder', '/1', 'Active', " + colorId1 + ");");
            int folderId1 = connection.QuerySingle<int>("SELECT FolderId FROM Folder WHERE FolderName = 'RootFolder'");
            connection.Execute("INSERT INTO Folder (ParentId, OwnerId, FolderName, FolderPath, FolderStatus, ColorId) VALUES (" + folderId1 + ", " + ownerId1 + ", 'ChildFolder1', '/1/" + folderId1 + "', 'Active', " + colorId1 + ");");
            int folderId2 = connection.QuerySingle<int>("SELECT FolderId FROM Folder WHERE FolderName = 'ChildFolder1'");
            connection.Execute("INSERT INTO Folder (ParentId, OwnerId, FolderName, FolderPath, FolderStatus, ColorId) VALUES (" + folderId1 + ", " + ownerId1 + ", 'ChildFolder2', '/1/" + folderId2 + "', 'Active', " + colorId1 + ");");

            // Insert into FileType (3 rows)
            connection.Execute("INSERT INTO FileType (FileTypeName, Icon) VALUES ('PDF', 'pdf_icon.png');");
            connection.Execute("INSERT INTO FileType (FileTypeName, Icon) VALUES ('Image', 'image_icon.png');");
            connection.Execute("INSERT INTO FileType (FileTypeName, Icon) VALUES ('Text', 'text_icon.png');");

            // Insert into UserFile (3 rows with hierarchical paths)
            int fileTypeId1 = connection.QuerySingle<int>("SELECT FileTypeId FROM FileType WHERE FileTypeName = 'PDF'");
            connection.Execute("INSERT INTO UserFile (FolderId, OwnerId, Size, UserFileName, UserFilePath, FileTypeId, UserFileStatus, ModifiedDate) VALUES (" + folderId2 + ", " + ownerId1 + ", 1024, 'Doc1.pdf', '/1/" + folderId2 + "/1', " + fileTypeId1 + ", 'Active', '2025-08-12 17:45:00');");
            int fileId1 = connection.QuerySingle<int>("SELECT FileId FROM UserFile WHERE UserFileName = 'Doc1.pdf'");
            int fileTypeId2 = connection.QuerySingle<int>("SELECT FileTypeId FROM FileType WHERE FileTypeName = 'Image'");
            connection.Execute("INSERT INTO UserFile (FolderId, OwnerId, Size, UserFileName, UserFilePath, FileTypeId, UserFileStatus, ModifiedDate) VALUES (" + folderId2 + ", " + ownerId1 + ", 2048, 'Pic1.jpg', '/1/" + folderId2 + "/2', " + fileTypeId2 + ", 'Active', '2025-08-12 17:45:00');");
            int fileId2 = connection.QuerySingle<int>("SELECT FileId FROM UserFile WHERE UserFileName = 'Pic1.jpg'");
            int fileTypeId3 = connection.QuerySingle<int>("SELECT FileTypeId FROM FileType WHERE FileTypeName = 'Text'");
            connection.Execute("INSERT INTO UserFile (FolderId, OwnerId, Size, UserFileName, UserFilePath, FileTypeId, UserFileStatus, ModifiedDate) VALUES (" + folderId2 + ", " + ownerId1 + ", 512, 'Note1.txt', '/1/" + folderId2 + "/3', " + fileTypeId3 + ", 'Active', '2025-08-12 17:45:00');");

            // Insert into Share (3 rows)
            int objectTypeId1 = connection.QuerySingle<int>("SELECT ObjectTypeId FROM ObjectType WHERE ObjectTypeName = 'Folder'");
            int objectTypeId2 = connection.QuerySingle<int>("SELECT ObjectTypeId FROM ObjectType WHERE ObjectTypeName = 'File'");
            connection.Execute("INSERT INTO Share (Sharer, ObjectId, ObjectTypeId, ShareUrl, UrlApprove) VALUES (" + ownerId1 + ", " + folderId1 + ", " + objectTypeId1 + ", 'http://share1', 1);");
            connection.Execute("INSERT INTO Share (Sharer, ObjectId, ObjectTypeId, ShareUrl, UrlApprove) VALUES (" + ownerId1 + ", " + folderId2 + ", " + objectTypeId1 + ", 'http://share2', 1);");
            connection.Execute("INSERT INTO Share (Sharer, ObjectId, ObjectTypeId, ShareUrl, UrlApprove) VALUES (" + ownerId1 + ", " + fileId1 + ", " + objectTypeId2 + ", 'http://share3', 0);");

            int ownerId2 = connection.QuerySingle<int>("SELECT UserId FROM Account WHERE UserName = 'Jane'");
            int ownerId3 = connection.QuerySingle<int>("SELECT UserId FROM Account WHERE UserName = 'Bob'");
            // Insert into SharedUser (3 rows)
            int permissionId3 = connection.QuerySingle<int>("SELECT PermissionId FROM Permission WHERE PermissionName = 'Viewer'");
            int shareId1 = connection.QuerySingle<int>("SELECT ShareId FROM Share WHERE ShareUrl = 'http://share1'");
            connection.Execute("INSERT INTO SharedUser (ShareId, UserId, PermissionId, CreatedAt, ModifiedAt) VALUES (" + shareId1 + ", " + ownerId2 + ", " + permissionId3 + ", '2025-08-12 17:45:00', '2025-08-12 17:45:00');");
            int shareId2 = connection.QuerySingle<int>("SELECT ShareId FROM Share WHERE ShareUrl = 'http://share2'");
            connection.Execute("INSERT INTO SharedUser (ShareId, UserId, PermissionId, CreatedAt, ModifiedAt) VALUES (" + shareId2 + ", " + ownerId2 + ", " + permissionId3 + ", '2025-08-12 17:45:00', '2025-08-12 17:45:00');");
            int shareId3 = connection.QuerySingle<int>("SELECT ShareId FROM Share WHERE ShareUrl = 'http://share3'");
            connection.Execute("INSERT INTO SharedUser (ShareId, UserId, PermissionId, CreatedAt, ModifiedAt) VALUES (" + shareId3 + ", " + ownerId2 + ", " + permissionId3 + ", '2025-08-12 17:45:00', '2025-08-12 17:45:00');");

            // Insert into FileVersion (3 rows)
            int fileId3 = connection.QuerySingle<int>("SELECT FileId FROM UserFile WHERE UserFileName = 'Note1.txt'");
            connection.Execute("INSERT INTO FileVersion (FileId, FileVersion, FileVersionPath, CreatedAt, UpdateBy, IsCurrent, Size) VALUES (" + fileId1 + ", 1, '/1/" + folderId2 + "/1_v1', '2025-08-12 17:45:00', " + ownerId1 + ", 1, 1024);");
            connection.Execute("INSERT INTO FileVersion (FileId, FileVersion, FileVersionPath, CreatedAt, UpdateBy, IsCurrent, Size) VALUES (" + fileId2 + ", 1, '/1/" + folderId2 + "/2_v1', '2025-08-12 17:45:00', " + ownerId1 + ", 1, 2048);");
            connection.Execute("INSERT INTO FileVersion (FileId, FileVersion, FileVersionPath, CreatedAt, UpdateBy, IsCurrent, Size) VALUES (" + fileId3 + ", 1, '/1/" + folderId2 + "/3_v1', '2025-08-12 17:45:00', " + ownerId1 + ", 1, 512);");

            // Insert into Trash (3 rows)
            connection.Execute("INSERT INTO Trash (ObjectId, ObjectTypeId, RemovedDatetime, UserId) VALUES (" + fileId1 + ", " + objectTypeId2 + ", '2025-08-12 17:45:00', " + ownerId1 + ");");
            connection.Execute("INSERT INTO Trash (ObjectId, ObjectTypeId, RemovedDatetime, UserId) VALUES (" + fileId2 + ", " + objectTypeId2 + ", '2025-08-12 17:45:00', " + ownerId1 + ");");
            connection.Execute("INSERT INTO Trash (ObjectId, ObjectTypeId, RemovedDatetime, UserId) VALUES (" + fileId3 + ", " + objectTypeId2 + ", '2025-08-12 17:45:00', " + ownerId1 + ");");

            // Insert into ProductItem (3 rows)
            connection.Execute("INSERT INTO ProductItem (ProductName, Cost, Duration) VALUES ('Plan1', 9.99, 30);");
            connection.Execute("INSERT INTO ProductItem (ProductName, Cost, Duration) VALUES ('Plan2', 19.99, 60);");
            connection.Execute("INSERT INTO ProductItem (ProductName, Cost, Duration) VALUES ('Plan3', 29.99, 90);");

            // Insert into Promotion (3 rows)
            connection.Execute("INSERT INTO Promotion (PromotionName, Discount, IsPercent) VALUES ('Promo1', 10, 1);");
            connection.Execute("INSERT INTO Promotion (PromotionName, Discount, IsPercent) VALUES ('Promo2', 5, 0);");
            connection.Execute("INSERT INTO Promotion (PromotionName, Discount, IsPercent) VALUES ('Promo3', 15, 1);");

            // Insert into UserProduct (3 rows)
            int productId1 = connection.QuerySingle<int>("SELECT ProductId FROM ProductItem WHERE ProductName = 'Plan1'");
            int promotionId1 = connection.QuerySingle<int>("SELECT PromotionId FROM Promotion WHERE PromotionName = 'Promo1'");
            connection.Execute("INSERT INTO UserProduct (UserId, ProductId, PayingDatetime, IsFirstPaying, PromotionId, EndDatetime) VALUES (" + ownerId1 + ", " + productId1 + ", '2025-08-12 17:45:00', 1, " + promotionId1 + ", '2025-09-11 17:45:00');");
            int productId2 = connection.QuerySingle<int>("SELECT ProductId FROM ProductItem WHERE ProductName = 'Plan2'");
            int promotionId2 = connection.QuerySingle<int>("SELECT PromotionId FROM Promotion WHERE PromotionName = 'Promo2'");
            connection.Execute("INSERT INTO UserProduct (UserId, ProductId, PayingDatetime, IsFirstPaying, PromotionId, EndDatetime) VALUES (" + ownerId1 + ", " + productId2 + ", '2025-08-12 17:45:00', 0, " + promotionId2 + ", '2025-10-11 17:45:00');");
            int productId3 = connection.QuerySingle<int>("SELECT ProductId FROM ProductItem WHERE ProductName = 'Plan3'");
            int promotionId3 = connection.QuerySingle<int>("SELECT PromotionId FROM Promotion WHERE PromotionName = 'Promo3'");
            connection.Execute("INSERT INTO UserProduct (UserId, ProductId, PayingDatetime, IsFirstPaying, PromotionId, EndDatetime) VALUES (" + ownerId1 + ", " + productId3 + ", '2025-08-12 17:45:00', 0, " + promotionId3 + ", '2025-11-11 17:45:00');");

            // Insert into BannedUser (3 rows)

            connection.Execute("INSERT INTO BannedUser (UserId, BannedAt, BannedUserId) VALUES (" + ownerId1 + ", '2025-08-12 17:45:00', " + ownerId2 + ");");
            connection.Execute("INSERT INTO BannedUser (UserId, BannedAt, BannedUserId) VALUES (" + ownerId1 + ", '2025-08-12 17:45:00', " + ownerId3 + ");");
            connection.Execute("INSERT INTO BannedUser (UserId, BannedAt, BannedUserId) VALUES (" + ownerId2 + ", '2025-08-12 17:45:00', " + ownerId3 + ");");

            // Insert into FavoriteObject (3 rows)
            connection.Execute("INSERT INTO FavoriteObject (OwnerId, ObjectId, ObjectTypeId) VALUES (" + ownerId1 + ", " + folderId1 + ", " + objectTypeId1 + ");");
            connection.Execute("INSERT INTO FavoriteObject (OwnerId, ObjectId, ObjectTypeId) VALUES (" + ownerId2 + ", " + folderId2 + ", " + objectTypeId1 + ");");
            connection.Execute("INSERT INTO FavoriteObject (OwnerId, ObjectId, ObjectTypeId) VALUES (" + ownerId3 + ", " + folderId1 + ", " + objectTypeId1 + ");");

            // Insert into ActionRecent (3 rows)
            connection.Execute("INSERT INTO ActionRecent (UserId, ObjectId, ObjectTypeId, ActionLog, ActionDateTime) VALUES (" + ownerId1 + ", " + folderId1 + ", " + objectTypeId1 + ", 'Created', '2025-08-12 17:45:00');");
            connection.Execute("INSERT INTO ActionRecent (UserId, ObjectId, ObjectTypeId, ActionLog, ActionDateTime) VALUES (" + ownerId2 + ", " + folderId2 + ", " + objectTypeId1 + ", 'Modified', '2025-08-12 17:45:00');");
            connection.Execute("INSERT INTO ActionRecent (UserId, ObjectId, ObjectTypeId, ActionLog, ActionDateTime) VALUES (" + ownerId3 + ", " + folderId1 + ", " + objectTypeId1 + ", 'Viewed', '2025-08-12 17:45:00');");

            // Insert into SearchHistory (3 rows)
            connection.Execute("INSERT INTO SearchHistory (UserId, SearchToken, SearchDatetime) VALUES (" + ownerId1 + ", 'folder', '2025-08-12 17:45:00');");
            connection.Execute("INSERT INTO SearchHistory (UserId, SearchToken, SearchDatetime) VALUES (" + ownerId2 + ", 'file', '2025-08-12 17:45:00');");
            connection.Execute("INSERT INTO SearchHistory (UserId, SearchToken, SearchDatetime) VALUES (" + ownerId3 + ", 'share', '2025-08-12 17:45:00');");

            // Insert into UserSession (3 rows)
            connection.Execute("INSERT INTO UserSession (UserId, Token, ExpiresAt) VALUES (" + ownerId1 + ", 'token1', '2025-08-13 17:45:00');");
            connection.Execute("INSERT INTO UserSession (UserId, Token, ExpiresAt) VALUES (" + ownerId2 + ", 'token2', '2025-08-13 17:45:00');");
            connection.Execute("INSERT INTO UserSession (UserId, Token, ExpiresAt) VALUES (" + ownerId3 + ", 'token3', '2025-08-13 17:45:00');");

            connection.Execute("INSERT INTO AppSettingKey (SettingKey, IsBoolean) VALUES ('StartPage', 0);");
            connection.Execute("INSERT INTO AppSettingKey (SettingKey, IsBoolean) VALUES ('ThemeMode', 0);");
            connection.Execute("INSERT INTO AppSettingKey (SettingKey, IsBoolean) VALUES ('Density', 0);");
            connection.Execute("INSERT INTO AppSettingKey (SettingKey, IsBoolean) VALUES ('OpenPDFMode', 0);");
            connection.Execute("INSERT INTO AppSettingKey (SettingKey, IsBoolean) VALUES ('ConvertUploadsToGoogleDocs', 1);");
            connection.Execute("INSERT INTO AppSettingKey (SettingKey, IsBoolean) VALUES ('OfflineModeEnabled', 1);");
            connection.Execute("INSERT INTO AppSettingKey (SettingKey, IsBoolean) VALUES ('ShowFilePreviewDetails', 1);");
            connection.Execute("INSERT INTO AppSettingKey (SettingKey, IsBoolean) VALUES ('EnableSoundEffects', 1);");

            // Insert into AppSettingOption (10 rows)
            connection.Execute("INSERT INTO AppSettingOption (SettingKeyId, SettingValue) VALUES (1, 'MyDrive');");
            connection.Execute("INSERT INTO AppSettingOption (SettingKeyId, SettingValue) VALUES (1, 'Home');");
            connection.Execute("INSERT INTO AppSettingOption (SettingKeyId, SettingValue) VALUES (2, 'Light');");
            connection.Execute("INSERT INTO AppSettingOption (SettingKeyId, SettingValue) VALUES (2, 'Dark');");
            connection.Execute("INSERT INTO AppSettingOption (SettingKeyId, SettingValue) VALUES (2, 'default');");
            connection.Execute("INSERT INTO AppSettingOption (SettingKeyId, SettingValue) VALUES (3, 'Medium');");
            connection.Execute("INSERT INTO AppSettingOption (SettingKeyId, SettingValue) VALUES (3, 'Low');");
            connection.Execute("INSERT INTO AppSettingOption (SettingKeyId, SettingValue) VALUES (3, 'High');");
            connection.Execute("INSERT INTO AppSettingOption (SettingKeyId, SettingValue) VALUES (4, 'Preview');");
            connection.Execute("INSERT INTO AppSettingOption (SettingKeyId, SettingValue) VALUES (4, 'New');");

            // Insert into UserSetting (12 rows)
            connection.Execute("INSERT INTO UserSetting (UserId, AppSettingKeyId, AppSettingOptionId) VALUES (1, 1, 2);");
            connection.Execute("INSERT INTO UserSetting (UserId, AppSettingKeyId, AppSettingOptionId) VALUES (1, 2, 3);"); 
            connection.Execute("INSERT INTO UserSetting (UserId, AppSettingKeyId, AppSettingOptionId) VALUES (1, 3, 6);"); 
            connection.Execute("INSERT INTO UserSetting (UserId, AppSettingKeyId, AppSettingOptionId) VALUES (1, 4, 10);"); 
            connection.Execute("INSERT INTO UserSetting (UserId, AppSettingKeyId, AppSettingOptionId) VALUES (2, 1, 2);"); 
            connection.Execute("INSERT INTO UserSetting (UserId, AppSettingKeyId, AppSettingOptionId) VALUES (2, 2, 3);");
            connection.Execute("INSERT INTO UserSetting (UserId, AppSettingKeyId, AppSettingOptionId) VALUES (2, 3, 7);"); 
            connection.Execute("INSERT INTO UserSetting (UserId, AppSettingKeyId, AppSettingOptionId) VALUES (2, 4, 9);");
            connection.Execute("INSERT INTO UserSetting (UserId, AppSettingKeyId, AppSettingOptionId) VALUES (3, 1, 2);"); 
            connection.Execute("INSERT INTO UserSetting (UserId, AppSettingKeyId, AppSettingOptionId) VALUES (3, 2, 5);");
            connection.Execute("INSERT INTO UserSetting (UserId, AppSettingKeyId, AppSettingOptionId) VALUES (3, 3, 8);");
            connection.Execute("INSERT INTO UserSetting (UserId, AppSettingKeyId, AppSettingOptionId) VALUES (3, 4, 9);");

            // Insert into FileContent (3 rows)
            connection.Execute("INSERT INTO FileContent (FileId, ContentChunk, ChunkIndex, DocumentLength) VALUES (" + fileId1 + ", 'Content1', 0, 1024);");
            connection.Execute("INSERT INTO FileContent (FileId, ContentChunk, ChunkIndex, DocumentLength) VALUES (" + fileId2 + ", 'Content2', 0, 2048);");
            connection.Execute("INSERT INTO FileContent (FileId, ContentChunk, ChunkIndex, DocumentLength) VALUES (" + fileId3 + ", 'Content3', 0, 512);");

            // Insert into Term (3 rows)
            int contentId1 = connection.QuerySingle<int>("SELECT ContentId FROM FileContent WHERE FileId = " + fileId1);
            connection.Execute("INSERT INTO Term (Term, FileContentId, CreatedAt) VALUES ('sample1', " + contentId1 + ", '2025-08-12 17:45:00');");
            int contentId2 = connection.QuerySingle<int>("SELECT ContentId FROM FileContent WHERE FileId = " + fileId2);
            connection.Execute("INSERT INTO Term (Term, FileContentId, CreatedAt) VALUES ('sample2', " + contentId2 + ", '2025-08-12 17:45:00');");
            int contentId3 = connection.QuerySingle<int>("SELECT ContentId FROM FileContent WHERE FileId = " + fileId3);
            connection.Execute("INSERT INTO Term (Term, FileContentId, CreatedAt) VALUES ('sample3', " + contentId3 + ", '2025-08-12 17:45:00');");

            // Insert into SearchIndex (3 rows)
            connection.Execute("INSERT INTO SearchIndex (FileContentId, Term, TermFrequency, TermPositions, Bm25Score, IDF) VALUES (" + contentId1 + ", 'sample1', 1, '0', 0.5, 0.1);");
            connection.Execute("INSERT INTO SearchIndex (FileContentId, Term, TermFrequency, TermPositions, Bm25Score, IDF) VALUES (" + contentId2 + ", 'sample2', 1, '0', 0.5, 0.1);");
            connection.Execute("INSERT INTO SearchIndex (FileContentId, Term, TermFrequency, TermPositions, Bm25Score, IDF) VALUES (" + contentId3 + ", 'sample3', 1, '0', 0.5, 0.1);");
        }
    }
}
