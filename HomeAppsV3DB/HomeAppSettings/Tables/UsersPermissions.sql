CREATE TABLE [HomeAppSettings].[UsersPermissions] (
    [UsersPermissionsID] INT           IDENTITY (1, 1) NOT NULL,
    [UserID]             INT           NOT NULL,
    [PermissionLevelID]  INT           NOT NULL,
    [ControllerName]     NVARCHAR (50) NOT NULL,
    [FunctionName]       NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_UsersPermissions] PRIMARY KEY CLUSTERED ([UsersPermissionsID] ASC),
    CONSTRAINT [FK_UsersPermissions_Users] FOREIGN KEY ([UsersPermissionsID]) REFERENCES [HomeAppSettings].[Users] ([UserID]),
    CONSTRAINT [FK_UsersPermissions_UsersPermissions] FOREIGN KEY ([UsersPermissionsID]) REFERENCES [HomeAppSettings].[UsersPermissions] ([UsersPermissionsID])
);

