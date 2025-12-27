CREATE TABLE [Kids].[ApiKeys] (
    [ApiKeyId]    INT             IDENTITY (1, 1) NOT NULL,
    [KeyName]     NVARCHAR (100)  NOT NULL,
    [KeyValue]    NVARCHAR (500)  NOT NULL,
    [Description] NVARCHAR (1000) NULL,
    [IsActive]    BIT             DEFAULT ((1)) NOT NULL,
    [ExpiresAt]   DATETIME2 (7)   NULL,
    [LastUsedAt]  DATETIME2 (7)   NULL,
    [IsDeleted]   BIT             DEFAULT ((0)) NOT NULL,
    [CreatedAt]   DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [ModifiedAt]  DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_ApiKeys] PRIMARY KEY CLUSTERED ([ApiKeyId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_ApiKeys_Active]
    ON [Kids].[ApiKeys]([IsActive] ASC, [IsDeleted] ASC)
    INCLUDE([ExpiresAt]);


GO
CREATE NONCLUSTERED INDEX [IX_ApiKeys_KeyValue]
    ON [Kids].[ApiKeys]([KeyValue] ASC) WHERE ([IsDeleted]=(0) AND [IsActive]=(1));

