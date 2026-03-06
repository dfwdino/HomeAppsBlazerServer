CREATE TABLE [dbo].[Log] (
    [LogID]      INT            IDENTITY (1, 1) NOT NULL,
    [Message]    NVARCHAR (MAX) NULL,
    [Level]      NVARCHAR (50)  NULL,
    [IpAddress]  NVARCHAR (50)  NULL,
    [CameFrom]   NVARCHAR (50)  NULL,
    [TimeStamp]  DATETIME       NULL,
    [Exception]  NVARCHAR (MAX) NULL,
    [Properties] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED ([LogID] ASC)
);

