CREATE TABLE [dbo].[Receipts] (
    [id]        INT            IDENTITY (1, 1) NOT NULL,
    [DateAdded] DATE           CONSTRAINT [DF_logs_DateAdded] DEFAULT (getdate()) NOT NULL,
    [Place]     NVARCHAR (50)  NULL,
    [Date]      NVARCHAR (50)  NULL,
    [Total]     NVARCHAR (50)  NULL,
    [tax]       NVARCHAR (50)  NULL,
    [item]      NVARCHAR (MAX) NULL,
    [itemprice] NVARCHAR (50)  NULL,
    [Notes]     NVARCHAR (MAX) NULL
);

