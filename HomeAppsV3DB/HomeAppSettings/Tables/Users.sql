CREATE TABLE [HomeAppSettings].[Users] (
    [UserID]    INT           IDENTITY (1, 1) NOT NULL,
    [FirstName] NVARCHAR (50) NOT NULL,
    [Username]  NVARCHAR (50) NOT NULL,
    [Password]  NVARCHAR (50) NOT NULL,
    [IsDeleted] BIT           CONSTRAINT [DEFAULT_Users_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CreatedOn] DATETIME      CONSTRAINT [DEFAULT_Users_CreatedOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([UserID] ASC)
);

