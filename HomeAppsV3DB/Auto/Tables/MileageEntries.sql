CREATE TABLE [Auto].[MileageEntries] (
    [EntryID]        INT             IDENTITY (1, 1) NOT NULL,
    [CarID]          INT             NOT NULL,
    [GasTypeID]      INT             NOT NULL,
    [StationID]      INT             NOT NULL,
    [EntryDate]      DATETIME        CONSTRAINT [DF_MileageEntries_EntryDate] DEFAULT (getdate()) NOT NULL,
    [Odometer]       DECIMAL (10, 1) NOT NULL,
    [Gallons]        DECIMAL (6, 3)  NOT NULL,
    [PricePerGallon] DECIMAL (6, 3)  NOT NULL,
    [Notes]          NVARCHAR (255)  NULL,
    [IsDeleted]      BIT             CONSTRAINT [DF__MileageEn__IsDel__047AA831] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__MileageE__F57BD2D75B7F2728] PRIMARY KEY CLUSTERED ([EntryID] ASC),
    CONSTRAINT [FK__MileageEn__CarID__056ECC6A] FOREIGN KEY ([CarID]) REFERENCES [Auto].[Cars] ([CarID]),
    CONSTRAINT [FK__MileageEn__GasTy__0662F0A3] FOREIGN KEY ([GasTypeID]) REFERENCES [Auto].[GasTypes] ([GasTypeID]),
    CONSTRAINT [FK__MileageEn__Stati__075714DC] FOREIGN KEY ([StationID]) REFERENCES [Auto].[GasStations] ([StationID])
);


GO
CREATE NONCLUSTERED INDEX [IX_MileageEntries_EntryDate]
    ON [Auto].[MileageEntries]([EntryDate] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_MileageEntries_CarID]
    ON [Auto].[MileageEntries]([CarID] ASC);

