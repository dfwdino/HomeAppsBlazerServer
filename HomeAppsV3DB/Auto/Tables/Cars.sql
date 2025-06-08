CREATE TABLE [Auto].[Cars] (
    [CarID]     INT           IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (50) NOT NULL,
    [DateAdded] DATETIME      CONSTRAINT [DF__Cars__DateAdded__7AF13DF7] DEFAULT (getdate()) NOT NULL,
    [IsDeleted] BIT           CONSTRAINT [DF__Cars__IsDeleted__7BE56230] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__Cars__68A0340EBC0783BF] PRIMARY KEY CLUSTERED ([CarID] ASC)
);

