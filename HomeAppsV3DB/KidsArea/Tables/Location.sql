CREATE TABLE [KidsArea].[Location] (
    [ChoreLocationId] INT           IDENTITY (1, 1) NOT NULL,
    [PlaceName]       NVARCHAR (50) NOT NULL,
    [IsDeleted]       BIT           CONSTRAINT [DF_Places_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Places] PRIMARY KEY CLUSTERED ([ChoreLocationId] ASC)
);

