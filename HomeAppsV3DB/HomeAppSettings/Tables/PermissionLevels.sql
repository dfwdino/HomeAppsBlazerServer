CREATE TABLE [HomeAppSettings].[PermissionLevels] (
    [PermissionLevelID] INT      IDENTITY (1, 1) NOT NULL,
    [PermissionName]    INT      NOT NULL,
    [IsDeleted]         CHAR (1) CONSTRAINT [DEFAULT_PermissionLevels_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_PermissionLevels] PRIMARY KEY CLUSTERED ([PermissionLevelID] ASC)
);

