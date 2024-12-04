IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [AppRoles] (
    [Id] uniqueidentifier NOT NULL,
    [Description] nvarchar(200) NOT NULL,
    [Name] nvarchar(max) NULL,
    [NormalizedName] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AppRoles] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AppUsers] (
    [Id] uniqueidentifier NOT NULL,
    [FirstName] nvarchar(50) NOT NULL,
    [LastName] nvarchar(50) NOT NULL,
    [Address] nvarchar(max) NOT NULL,
    [Gender] int NOT NULL,
    [DateOfBirth] nvarchar(max) NOT NULL,
    [CreatedDate] nvarchar(max) NOT NULL,
    [RefreshToken] nvarchar(max) NOT NULL,
    [RefreshTokenExpiryTime] datetime2 NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [UserLinks] nvarchar(max) NOT NULL,
    [UserName] nvarchar(max) NULL,
    [NormalizedUserName] nvarchar(max) NULL,
    [Email] nvarchar(max) NULL,
    [NormalizedEmail] nvarchar(max) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AppUsers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [RoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] uniqueidentifier NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_RoleClaims] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [UserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] uniqueidentifier NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_UserClaims] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [UserLogins] (
    [UserId] uniqueidentifier NOT NULL,
    [LoginProvider] nvarchar(max) NULL,
    [ProviderKey] nvarchar(max) NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    CONSTRAINT [PK_UserLogins] PRIMARY KEY ([UserId])
);
GO

CREATE TABLE [UserRoles] (
    [UserId] uniqueidentifier NOT NULL,
    [RoleId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_UserRoles] PRIMARY KEY ([UserId], [RoleId])
);
GO

CREATE TABLE [UserTokens] (
    [UserId] uniqueidentifier NOT NULL,
    [LoginProvider] nvarchar(max) NULL,
    [Name] nvarchar(max) NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_UserTokens] PRIMARY KEY ([UserId])
);
GO

CREATE TABLE [Tests] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(200) NOT NULL,
    [ImageUrl] nvarchar(500) NOT NULL,
    [UpdatedDate] nvarchar(max) NOT NULL,
    [CreatedDate] nvarchar(max) NOT NULL,
    [Description] nvarchar(1000) NOT NULL,
    [Source] nvarchar(500) NOT NULL,
    [Duration] int NOT NULL,
    [AuthorId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_Tests] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Tests_AppUsers_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [AppUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Questions] (
    [Id] uniqueidentifier NOT NULL,
    [Content] nvarchar(1000) NOT NULL,
    [ImageUrl] nvarchar(max) NOT NULL,
    [AnswerA] nvarchar(500) NOT NULL,
    [AnswerB] nvarchar(500) NOT NULL,
    [AnswerC] nvarchar(500) NOT NULL,
    [AnswerD] nvarchar(500) NOT NULL,
    [CorrectAnswer] int NOT NULL,
    [TestId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_Questions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Questions_Tests_TestId] FOREIGN KEY ([TestId]) REFERENCES [Tests] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [TestResults] (
    [Id] uniqueidentifier NOT NULL,
    [Score] decimal(18,2) NOT NULL,
    [Correct] int NOT NULL,
    [StartTime] nvarchar(max) NOT NULL,
    [EndTime] nvarchar(max) NOT NULL,
    [UsedTime] int NOT NULL,
    [TestId] uniqueidentifier NOT NULL,
    [UserId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_TestResults] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TestResults_AppUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AppUsers] ([Id]),
    CONSTRAINT [FK_TestResults_Tests_TestId] FOREIGN KEY ([TestId]) REFERENCES [Tests] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Questions_TestId] ON [Questions] ([TestId]);
GO

CREATE INDEX [IX_TestResults_TestId] ON [TestResults] ([TestId]);
GO

CREATE INDEX [IX_TestResults_UserId] ON [TestResults] ([UserId]);
GO

CREATE INDEX [IX_Tests_AuthorId] ON [Tests] ([AuthorId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240821144855_init', N'6.0.30');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [TestResults] DROP CONSTRAINT [FK_TestResults_AppUsers_UserId];
GO

ALTER TABLE [TestResults] DROP CONSTRAINT [FK_TestResults_Tests_TestId];
GO

ALTER TABLE [Tests] DROP CONSTRAINT [FK_Tests_AppUsers_AuthorId];
GO

DROP INDEX [IX_Tests_AuthorId] ON [Tests];
GO

DROP INDEX [IX_TestResults_TestId] ON [TestResults];
GO

DROP INDEX [IX_TestResults_UserId] ON [TestResults];
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Tests]') AND [c].[name] = N'AuthorId');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Tests] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Tests] ALTER COLUMN [AuthorId] nvarchar(max) NOT NULL;
GO

ALTER TABLE [Tests] ADD [AppUserId] uniqueidentifier NULL;
GO

ALTER TABLE [Tests] ADD [AuthorName] nvarchar(max) NOT NULL DEFAULT N'';
GO

ALTER TABLE [Tests] ADD [NumberOfAttempts] int NOT NULL DEFAULT 0;
GO

ALTER TABLE [Tests] ADD [NumberOfQuestions] int NOT NULL DEFAULT 0;
GO

ALTER TABLE [TestResults] ADD [AppUserId] uniqueidentifier NULL;
GO

ALTER TABLE [TestResults] ADD [TestName] nvarchar(max) NOT NULL DEFAULT N'';
GO

ALTER TABLE [TestResults] ADD [UserName] nvarchar(max) NOT NULL DEFAULT N'';
GO

CREATE INDEX [IX_Tests_AppUserId] ON [Tests] ([AppUserId]);
GO

CREATE INDEX [IX_TestResults_AppUserId] ON [TestResults] ([AppUserId]);
GO

ALTER TABLE [TestResults] ADD CONSTRAINT [FK_TestResults_AppUsers_AppUserId] FOREIGN KEY ([AppUserId]) REFERENCES [AppUsers] ([Id]);
GO

ALTER TABLE [Tests] ADD CONSTRAINT [FK_Tests_AppUsers_AppUserId] FOREIGN KEY ([AppUserId]) REFERENCES [AppUsers] ([Id]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240823131323_update-user-propertities', N'6.0.30');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AppUsers]') AND [c].[name] = N'RefreshToken');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [AppUsers] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [AppUsers] ALTER COLUMN [RefreshToken] nvarchar(max) NULL;
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AppUsers]') AND [c].[name] = N'LastName');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [AppUsers] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [AppUsers] ALTER COLUMN [LastName] nvarchar(50) NULL;
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AppUsers]') AND [c].[name] = N'FirstName');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [AppUsers] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [AppUsers] ALTER COLUMN [FirstName] nvarchar(50) NULL;
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AppUsers]') AND [c].[name] = N'DateOfBirth');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [AppUsers] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [AppUsers] ALTER COLUMN [DateOfBirth] nvarchar(max) NULL;
GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AppUsers]') AND [c].[name] = N'Address');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [AppUsers] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [AppUsers] ALTER COLUMN [Address] nvarchar(50) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240824184327_update-user-config', N'6.0.30');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AppUsers]') AND [c].[name] = N'UserName');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [AppUsers] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [AppUsers] ALTER COLUMN [UserName] nvarchar(max) NOT NULL;
ALTER TABLE [AppUsers] ADD DEFAULT N'' FOR [UserName];
GO

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AppUsers]') AND [c].[name] = N'Email');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [AppUsers] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [AppUsers] ALTER COLUMN [Email] nvarchar(max) NOT NULL;
ALTER TABLE [AppUsers] ADD DEFAULT N'' FOR [Email];
GO

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AppUsers]') AND [c].[name] = N'CreatedDate');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [AppUsers] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [AppUsers] ALTER COLUMN [CreatedDate] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240824185129_update-user-config-2', N'6.0.30');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240824185356_update-user-config-3', N'6.0.30');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var9 sysname;
SELECT @var9 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AppUsers]') AND [c].[name] = N'UserLinks');
IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [AppUsers] DROP CONSTRAINT [' + @var9 + '];');
ALTER TABLE [AppUsers] ALTER COLUMN [UserLinks] nvarchar(max) NULL;
GO

DECLARE @var10 sysname;
SELECT @var10 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AppUsers]') AND [c].[name] = N'Gender');
IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [AppUsers] DROP CONSTRAINT [' + @var10 + '];');
ALTER TABLE [AppUsers] ADD DEFAULT 2 FOR [Gender];
GO

DECLARE @var11 sysname;
SELECT @var11 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AppUsers]') AND [c].[name] = N'Description');
IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [AppUsers] DROP CONSTRAINT [' + @var11 + '];');
ALTER TABLE [AppUsers] ALTER COLUMN [Description] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240824185612_update-user-config-4', N'6.0.30');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Tests] DROP CONSTRAINT [FK_Tests_AppUsers_AppUserId];
GO

DROP INDEX [IX_Tests_AppUserId] ON [Tests];
GO

DECLARE @var12 sysname;
SELECT @var12 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Tests]') AND [c].[name] = N'AppUserId');
IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [Tests] DROP CONSTRAINT [' + @var12 + '];');
ALTER TABLE [Tests] DROP COLUMN [AppUserId];
GO

DECLARE @var13 sysname;
SELECT @var13 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Tests]') AND [c].[name] = N'AuthorId');
IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [Tests] DROP CONSTRAINT [' + @var13 + '];');
ALTER TABLE [Tests] ALTER COLUMN [AuthorId] uniqueidentifier NOT NULL;
GO

CREATE INDEX [IX_Tests_AuthorId] ON [Tests] ([AuthorId]);
GO

ALTER TABLE [Tests] ADD CONSTRAINT [FK_Tests_AppUsers_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [AppUsers] ([Id]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240824193850_update-user', N'6.0.30');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240825082816_update-test-image-nullable', N'6.0.30');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var14 sysname;
SELECT @var14 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Tests]') AND [c].[name] = N'ImageUrl');
IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [Tests] DROP CONSTRAINT [' + @var14 + '];');
ALTER TABLE [Tests] ALTER COLUMN [ImageUrl] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240825083014_update-test-image-nullable-2', N'6.0.30');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var15 sysname;
SELECT @var15 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Questions]') AND [c].[name] = N'AnswerD');
IF @var15 IS NOT NULL EXEC(N'ALTER TABLE [Questions] DROP CONSTRAINT [' + @var15 + '];');
ALTER TABLE [Questions] ALTER COLUMN [AnswerD] nvarchar(500) NULL;
GO

DECLARE @var16 sysname;
SELECT @var16 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Questions]') AND [c].[name] = N'AnswerC');
IF @var16 IS NOT NULL EXEC(N'ALTER TABLE [Questions] DROP CONSTRAINT [' + @var16 + '];');
ALTER TABLE [Questions] ALTER COLUMN [AnswerC] nvarchar(500) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240825083326_update-question-ans-nullable', N'6.0.30');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var17 sysname;
SELECT @var17 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Questions]') AND [c].[name] = N'ImageUrl');
IF @var17 IS NOT NULL EXEC(N'ALTER TABLE [Questions] DROP CONSTRAINT [' + @var17 + '];');
ALTER TABLE [Questions] ALTER COLUMN [ImageUrl] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240825083453_update-question-image-nullable', N'6.0.30');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var18 sysname;
SELECT @var18 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TestResults]') AND [c].[name] = N'UserId');
IF @var18 IS NOT NULL EXEC(N'ALTER TABLE [TestResults] DROP CONSTRAINT [' + @var18 + '];');
ALTER TABLE [TestResults] ALTER COLUMN [UserId] nvarchar(max) NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240828153647_change-test-result-prop', N'6.0.30');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [TestResults] DROP CONSTRAINT [FK_TestResults_AppUsers_AppUserId];
GO

DROP INDEX [IX_TestResults_AppUserId] ON [TestResults];
GO

DECLARE @var19 sysname;
SELECT @var19 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TestResults]') AND [c].[name] = N'AppUserId');
IF @var19 IS NOT NULL EXEC(N'ALTER TABLE [TestResults] DROP CONSTRAINT [' + @var19 + '];');
ALTER TABLE [TestResults] DROP COLUMN [AppUserId];
GO

CREATE TABLE [SavedTests] (
    [UserId] uniqueidentifier NOT NULL,
    [TestId] uniqueidentifier NOT NULL,
    [MarkedAt] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_UserInTests] PRIMARY KEY ([UserId], [TestId]),
    CONSTRAINT [FK_UserInTests_AppUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AppUsers] ([Id]),
    CONSTRAINT [FK_UserInTests_Tests_TestId] FOREIGN KEY ([TestId]) REFERENCES [Tests] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_UserInTests_TestId] ON [SavedTests] ([TestId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240831091605_add-user-in-test', N'6.0.30');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Tests] ADD [IsPrivate] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240901000858_add-test-prop', N'6.0.30');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

EXEC sp_rename N'[UserInTests]', N'SavedTests';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240910151553_change-saved-test-table-name', N'6.0.30');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [AppUsers] ADD [ImageUrl] nvarchar(max) NOT NULL DEFAULT N'';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240913141302_user-image', N'6.0.30');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var20 sysname;
SELECT @var20 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AppUsers]') AND [c].[name] = N'ImageUrl');
IF @var20 IS NOT NULL EXEC(N'ALTER TABLE [AppUsers] DROP CONSTRAINT [' + @var20 + '];');
ALTER TABLE [AppUsers] ALTER COLUMN [ImageUrl] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241003154203_change-user-image-nullable', N'6.0.30');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Images] (
    [Name] nvarchar(450) NOT NULL,
    [Owner] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Images] PRIMARY KEY ([Name])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241015221123_add-image-table', N'6.0.30');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Comments] (
    [Id] uniqueidentifier NOT NULL,
    [Content] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [Likes] int NOT NULL,
    [UserId] uniqueidentifier NOT NULL,
    [TestId] uniqueidentifier NOT NULL,
    [CommentId] uniqueidentifier NULL,
    CONSTRAINT [PK_Comments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Comments_AppUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AppUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Comments_Comments_CommentId] FOREIGN KEY ([CommentId]) REFERENCES [Comments] ([Id]),
    CONSTRAINT [FK_Comments_Tests_TestId] FOREIGN KEY ([TestId]) REFERENCES [Tests] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Comments_CommentId] ON [Comments] ([CommentId]);
GO

CREATE INDEX [IX_Comments_TestId] ON [Comments] ([TestId]);
GO

CREATE INDEX [IX_Comments_UserId] ON [Comments] ([UserId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241021073408_add-comment-entity', N'6.0.30');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Comments] DROP CONSTRAINT [FK_Comments_Comments_CommentId];
GO

DROP INDEX [IX_Comments_CommentId] ON [Comments];
GO

EXEC sp_rename N'[Comments].[CommentId]', N'ParentId', N'COLUMN';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241021141112_change-comment-config', N'6.0.30');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [AppUsers] ADD [Active] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

ALTER TABLE [AppUsers] ADD [Likes] int NOT NULL DEFAULT 0;
GO

CREATE TABLE [UserLikes] (
    [LikedUserId] uniqueidentifier NOT NULL,
    [UserId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_UserLikes] PRIMARY KEY ([LikedUserId], [UserId])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241115000033_add-user-like-and-active', N'6.0.30');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Summaries] (
    [Date] date NOT NULL,
    [AccessCount] int NOT NULL,
    [RequestCount] int NOT NULL,
    [TestCompletionCount] int NOT NULL,
    [CommentCount] int NOT NULL,
    [UserCount] int NOT NULL,
    CONSTRAINT [PK_Summaries] PRIMARY KEY ([Date])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241121040544_add-summary', N'6.0.30');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var21 sysname;
SELECT @var21 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Summaries]') AND [c].[name] = N'RequestCount');
IF @var21 IS NOT NULL EXEC(N'ALTER TABLE [Summaries] DROP CONSTRAINT [' + @var21 + '];');
ALTER TABLE [Summaries] DROP COLUMN [RequestCount];
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241122224130_drop-summary-request-count', N'6.0.30');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Categories] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [ParentId] uniqueidentifier NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Categories_Categories_ParentId] FOREIGN KEY ([ParentId]) REFERENCES [Categories] ([Id]) ON DELETE NO ACTION
);
GO

CREATE UNIQUE INDEX [IX_Categories_Name] ON [Categories] ([Name]);
GO

CREATE INDEX [IX_Categories_ParentId] ON [Categories] ([ParentId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241124050157_add_category', N'6.0.30');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Categories] ADD [Slug] nvarchar(max) NOT NULL DEFAULT N'';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241125235306_add-category-slug', N'6.0.30');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Tests] ADD [CategorySlug] nvarchar(max) NOT NULL DEFAULT N'';
GO

DECLARE @var22 sysname;
SELECT @var22 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Categories]') AND [c].[name] = N'Slug');
IF @var22 IS NOT NULL EXEC(N'ALTER TABLE [Categories] DROP CONSTRAINT [' + @var22 + '];');
ALTER TABLE [Categories] ALTER COLUMN [Slug] nvarchar(450) NOT NULL;
GO

CREATE UNIQUE INDEX [IX_Categories_Slug] ON [Categories] ([Slug]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241126050530_add-test-category', N'6.0.30');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Questions] ADD [Order] int NOT NULL DEFAULT 0;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241129110310_add-question-order', N'6.0.30');
GO

COMMIT;
GO

