CREATE TABLE [workout].[CardioSessions] (
    [Id]             INT             IDENTITY (1, 1) NOT NULL,
    [DateTime]       DATETIME        NOT NULL,
    [Duration]       TIME (7)        NOT NULL,
    [CaloriesBurned] INT             NOT NULL,
    [CardioTypeId]   INT             NOT NULL,
    [LocationId]     INT             NOT NULL,
    [Distance]       DECIMAL (10, 2) DEFAULT ((0.00)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([CardioTypeId]) REFERENCES [workout].[CardioTypes] ([Id]),
    FOREIGN KEY ([LocationId]) REFERENCES [workout].[Locations] ([Id])
);

