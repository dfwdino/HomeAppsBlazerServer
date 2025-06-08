CREATE TABLE [Auto].[GasTypes] (
    [GasTypeID] INT           IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (50) NOT NULL,
    [IsDeleted] BIT           CONSTRAINT [DF__GasTypes__IsDele__7EC1CEDB] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__GasTypes__01491BD3FC1C08C4] PRIMARY KEY CLUSTERED ([GasTypeID] ASC)
);

