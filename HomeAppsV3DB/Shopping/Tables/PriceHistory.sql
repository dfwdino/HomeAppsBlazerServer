CREATE TABLE [Shopping].[PriceHistory] (
    [PriceHistoryID] INT            IDENTITY (1, 1) NOT NULL,
    [Amount]         DECIMAL (5, 2) NULL,
    [PriceDate]      DATE           CONSTRAINT [DF_PriceHistory_PriceDate] DEFAULT (getdate()) NOT NULL,
    [ItemID]         INT            NOT NULL,
    [StoreID]        INT            NULL,
    CONSTRAINT [PK_PriceHistory] PRIMARY KEY CLUSTERED ([PriceHistoryID] ASC),
    CONSTRAINT [FK_PriceHistory_ShoppingItems] FOREIGN KEY ([ItemID]) REFERENCES [Shopping].[ShoppingItems] ([ShoppingItemID]),
    CONSTRAINT [FK_PriceHistory_ShoppingStores] FOREIGN KEY ([StoreID]) REFERENCES [Shopping].[ShoppingStores] ([ShoppingStoreID])
);

