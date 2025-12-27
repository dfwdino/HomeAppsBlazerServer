CREATE TABLE [Kids].[ChoreItems] (
    [ChoreItemId] INT             IDENTITY (1, 1) NOT NULL,
    [ChoreName]   NVARCHAR (100)  NOT NULL,
    [Price]       DECIMAL (10, 2) NOT NULL,
    [Description] NVARCHAR (255)  NULL,
    [IsDeleted]   BIT             DEFAULT ((0)) NOT NULL,
    [CreatedAt]   DATETIME2 (7)   DEFAULT (sysdatetime()) NOT NULL,
    [ModifiedAt]  DATETIME2 (7)   DEFAULT (sysdatetime()) NOT NULL,
    PRIMARY KEY CLUSTERED ([ChoreItemId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_ChoreItems_IsDeleted]
    ON [Kids].[ChoreItems]([IsDeleted] ASC);

