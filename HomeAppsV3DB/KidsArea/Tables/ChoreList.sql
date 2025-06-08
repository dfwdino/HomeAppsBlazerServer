CREATE TABLE [KidsArea].[ChoreList] (
    [ChoreHistoryID] INT      IDENTITY (1, 1) NOT NULL,
    [KidsChoreID]    INT      NOT NULL,
    [KidsNameID]     INT      NOT NULL,
    [DoneDate]       DATETIME CONSTRAINT [DF_ChoreList_DoneDate] DEFAULT (getdate()) NULL,
    [IsDeleted]      BIT      CONSTRAINT [DEFAULT_ChoreHistory_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ChoreHistory] PRIMARY KEY CLUSTERED ([ChoreHistoryID] ASC),
    CONSTRAINT [FK_ChoreList_Chores] FOREIGN KEY ([KidsChoreID]) REFERENCES [KidsArea].[Chores] ([ChoreID]),
    CONSTRAINT [FK_ChoreList_KidsName] FOREIGN KEY ([KidsNameID]) REFERENCES [KidsArea].[KidsName] ([IDKidsName])
);

