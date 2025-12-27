CREATE TABLE [Kids].[WeeklyPaymentStatuses] (
    [WeeklyPaymentStatusId] INT           IDENTITY (1, 1) NOT NULL,
    [KidId]                 INT           NOT NULL,
    [WeekStartDate]         DATETIME2 (7) NOT NULL,
    [IsPaid]                BIT           NOT NULL,
    PRIMARY KEY CLUSTERED ([WeeklyPaymentStatusId] ASC),
    CONSTRAINT [FK_WeeklyPaymentStatuses_Kids_KidId] FOREIGN KEY ([KidId]) REFERENCES [Kids].[Kids] ([KidId])
);

