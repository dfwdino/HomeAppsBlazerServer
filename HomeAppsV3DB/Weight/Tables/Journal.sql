CREATE TABLE [Weight].[Journal] (
    [ID]        INT            IDENTITY (1, 1) NOT NULL,
    [Pounds]    DECIMAL (6, 2) NULL,
    [WaistSize] DECIMAL (6, 2) NULL,
    [Date]      DATETIME       CONSTRAINT [DF_Journal_Date] DEFAULT (getdate()) NOT NULL,
    [IsDeleted] BIT            CONSTRAINT [DF_Journal_IsDeleted] DEFAULT ((0)) NOT NULL,
    [Notes]     NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Journal] PRIMARY KEY CLUSTERED ([ID] ASC)
);

