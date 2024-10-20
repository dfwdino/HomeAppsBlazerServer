CREATE TABLE [KidsArea].[ChoreList] (
    [ChoreHistoryID] INT      IDENTITY (1, 1) NOT NULL,
    [KidsChoreID]    INT      NOT NULL,
    [StartDate]      DATETIME CONSTRAINT [DF_ChoreList_StartDate] DEFAULT (getdate()) NULL,
    [RequireDate]    DATETIME NULL,
    [DoneDate]       DATETIME CONSTRAINT [DEFAULT_ChoreHistory_DateDone] DEFAULT (getdate()) NOT NULL,
    [IsDeleted]      BIT      CONSTRAINT [DEFAULT_ChoreHistory_IsDeleted] DEFAULT ((0)) NOT NULL,
    [KidsNameID]     INT      NOT NULL,
    CONSTRAINT [PK_ChoreHistory] PRIMARY KEY CLUSTERED ([ChoreHistoryID] ASC)
);

