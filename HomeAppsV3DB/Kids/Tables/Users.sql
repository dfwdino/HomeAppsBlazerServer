CREATE TABLE [Kids].[Users] (
    [UserId]       INT            IDENTITY (1, 1) NOT NULL,
    [Username]     NVARCHAR (100) NOT NULL,
    [PasswordHash] NVARCHAR (256) NOT NULL,
    [Role]         NVARCHAR (20)  NOT NULL,
    [IsDeleted]    BIT            DEFAULT ((0)) NOT NULL,
    [CreatedAt]    DATETIME2 (7)  DEFAULT (sysdatetime()) NOT NULL,
    [ModifiedAt]   DATETIME2 (7)  DEFAULT (sysdatetime()) NOT NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC),
    UNIQUE NONCLUSTERED ([Username] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Users_IsDeleted]
    ON [Kids].[Users]([IsDeleted] ASC);


GO

CREATE TRIGGER Kids.trg_Users_ModifiedAt
ON Kids.Users
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Kids.Users
    SET ModifiedAt = SYSDATETIME()
    FROM Inserted
    WHERE Kids.Users.UserId = Inserted.UserId;
END