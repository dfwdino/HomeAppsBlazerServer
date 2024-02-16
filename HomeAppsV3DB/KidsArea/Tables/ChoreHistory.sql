CREATE TABLE [KidsArea].[ChoreHistory] (
    [ChoreHistoryID] INT      IDENTITY (1, 1) NOT NULL,
    [KidsChoreID]    INT      NOT NULL,
    [DateDone]       DATETIME CONSTRAINT [DEFAULT_ChoreHistory_DateDone] DEFAULT (getdate()) NOT NULL,
    [IsDeleted]      BIT      CONSTRAINT [DEFAULT_ChoreHistory_IsDeleted] DEFAULT ((0)) NOT NULL,
    [KidsNameID]     INT      NOT NULL,
    CONSTRAINT [PK_ChoreHistory] PRIMARY KEY CLUSTERED ([ChoreHistoryID] ASC),
    CONSTRAINT [FK_ChoreHistory_Chores] FOREIGN KEY ([KidsChoreID]) REFERENCES [KidsArea].[Chores] ([ChoreID]),
    CONSTRAINT [FK_ChoreHistory_KidsName] FOREIGN KEY ([KidsNameID]) REFERENCES [KidsArea].[KidsName] ([IDKidsName])
);

