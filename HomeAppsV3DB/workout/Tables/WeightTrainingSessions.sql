CREATE TABLE [workout].[WeightTrainingSessions] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL,
    [DateTime]      DATETIME        NOT NULL,
    [Reps]          INT             NOT NULL,
    [Weight]        DECIMAL (10, 2) NOT NULL,
    [WorkoutTypeId] INT             NOT NULL,
    [LocationId]    INT             NOT NULL,
    [Sets]          INT             DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([LocationId]) REFERENCES [workout].[Locations] ([Id]),
    FOREIGN KEY ([WorkoutTypeId]) REFERENCES [workout].[WorkoutTypes] ([Id])
);

