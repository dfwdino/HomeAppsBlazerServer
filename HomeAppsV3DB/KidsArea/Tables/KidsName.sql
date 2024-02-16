CREATE TABLE [KidsArea].[KidsName] (
    [IDKidsName] INT           IDENTITY (1, 1) NOT NULL,
    [KidName]    NVARCHAR (50) NOT NULL,
    [IsDeleted]  BIT           CONSTRAINT [DEFAULT_KidsName_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_KidsName] PRIMARY KEY CLUSTERED ([IDKidsName] ASC)
);

