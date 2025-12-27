CREATE TABLE [Kids].[Locations] (
    [LocationId]   INT            IDENTITY (1, 1) NOT NULL,
    [LocationName] NVARCHAR (100) NOT NULL,
    [Description]  NVARCHAR (255) NULL,
    [IsDeleted]    BIT            DEFAULT ((0)) NOT NULL,
    [CreatedAt]    DATETIME2 (7)  DEFAULT (sysdatetime()) NOT NULL,
    [ModifiedAt]   DATETIME2 (7)  DEFAULT (sysdatetime()) NOT NULL,
    PRIMARY KEY CLUSTERED ([LocationId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Locations_IsDeleted]
    ON [Kids].[Locations]([IsDeleted] ASC);


GO

CREATE TRIGGER Kids.trg_Locations_ModifiedAt
ON Kids.Locations
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Kids.Locations
    SET ModifiedAt = SYSDATETIME()
    FROM Inserted
    WHERE Kids.Locations.LocationId = Inserted.LocationId;
END