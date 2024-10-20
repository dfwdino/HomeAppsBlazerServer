CREATE TABLE [KidsArea].[Chores] (
    [ChoreID]         INT            IDENTITY (1, 1) NOT NULL,
    [ChoreName]       NVARCHAR (50)  NOT NULL,
    [Amount]          DECIMAL (6, 2) CONSTRAINT [DEFAULT_Chores_Amount] DEFAULT ((0.0)) NOT NULL,
    [IsDeleted]       BIT            CONSTRAINT [DEFAULT_Chores_IsDeleted] DEFAULT ((0)) NOT NULL,
    [ChoreLocationID] INT            NULL,
    CONSTRAINT [PK_Chores] PRIMARY KEY CLUSTERED ([ChoreID] ASC),
    CONSTRAINT [FK_Chores_Location] FOREIGN KEY ([ChoreLocationID]) REFERENCES [KidsArea].[Location] ([ChoreLocationId])
);

