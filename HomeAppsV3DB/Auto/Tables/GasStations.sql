CREATE TABLE [Auto].[GasStations] (
    [StationID] INT            IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (100) NOT NULL,
    [IsDeleted] BIT            CONSTRAINT [DF__GasStatio__IsDel__019E3B86] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__GasStati__E0D8A6DD42CBA481] PRIMARY KEY CLUSTERED ([StationID] ASC)
);

