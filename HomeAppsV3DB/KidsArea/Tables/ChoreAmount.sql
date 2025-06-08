CREATE TABLE [KidsArea].[ChoreAmount] (
    [ID]        INT            IDENTITY (1, 1) NOT NULL,
    [ChoreID]   INT            NOT NULL,
    [IsDeleted] BIT            CONSTRAINT [DF_ChoreAmount_IsDeleted] DEFAULT ((0)) NOT NULL,
    [Amount]    DECIMAL (8, 2) NULL,
    CONSTRAINT [PK_ChoreAmount] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ChoreAmount_Chores] FOREIGN KEY ([ChoreID]) REFERENCES [KidsArea].[Chores] ([ChoreID])
);

