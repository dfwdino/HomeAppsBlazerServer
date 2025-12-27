CREATE TABLE [Kids].[Kids] (
    [KidId]      INT            IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (100) NOT NULL,
    [Age]        INT            NULL,
    [Grade]      NVARCHAR (20)  NULL,
    [IsDeleted]  BIT            DEFAULT ((0)) NOT NULL,
    [CreatedAt]  DATETIME2 (7)  DEFAULT (sysdatetime()) NOT NULL,
    [ModifiedAt] DATETIME2 (7)  DEFAULT (sysdatetime()) NOT NULL,
    PRIMARY KEY CLUSTERED ([KidId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Kids_IsDeleted]
    ON [Kids].[Kids]([IsDeleted] ASC);


GO
CREATE TRIGGER Kids.trg_Kids_ModifiedAt
ON Kids.Kids
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Kids.Kids
    SET ModifiedAt = SYSDATETIME()
    FROM Inserted
    WHERE Kids.Kids.KidId = Inserted.KidId;
END