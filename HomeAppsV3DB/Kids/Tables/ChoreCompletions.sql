CREATE TABLE [Kids].[ChoreCompletions] (
    [ChoreCompletionId]  INT             IDENTITY (1, 1) NOT NULL,
    [KidId]              INT             NOT NULL,
    [ChoreItemId]        INT             NOT NULL,
    [LocationId]         INT             NOT NULL,
    [CompletionDateTime] DATETIME2 (7)   NOT NULL,
    [Notes]              NVARCHAR (255)  NULL,
    [IsDeleted]          BIT             DEFAULT ((0)) NOT NULL,
    [CreatedAt]          DATETIME2 (7)   DEFAULT (sysdatetime()) NOT NULL,
    [ModifiedAt]         DATETIME2 (7)   DEFAULT (sysdatetime()) NOT NULL,
    [Price]              DECIMAL (18, 2) DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([ChoreCompletionId] ASC),
    CONSTRAINT [FK_ChoreCompletions_ChoreItem] FOREIGN KEY ([ChoreItemId]) REFERENCES [Kids].[ChoreItems] ([ChoreItemId]),
    CONSTRAINT [FK_ChoreCompletions_Kid] FOREIGN KEY ([KidId]) REFERENCES [Kids].[Kids] ([KidId]),
    CONSTRAINT [FK_ChoreCompletions_Location] FOREIGN KEY ([LocationId]) REFERENCES [Kids].[Locations] ([LocationId])
);


GO
CREATE NONCLUSTERED INDEX [IX_ChoreCompletions_CompletionDateTime]
    ON [Kids].[ChoreCompletions]([CompletionDateTime] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ChoreCompletions_LocationId]
    ON [Kids].[ChoreCompletions]([LocationId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ChoreCompletions_ChoreItemId]
    ON [Kids].[ChoreCompletions]([ChoreItemId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ChoreCompletions_KidId]
    ON [Kids].[ChoreCompletions]([KidId] ASC);

