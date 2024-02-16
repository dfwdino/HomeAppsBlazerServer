CREATE TABLE [Shopping].[ShoppingItemList] (
    [ShoppingItemListID] INT  IDENTITY (1, 1) NOT NULL,
    [ShoppingItemID]     INT  NOT NULL,
    [ShoppingStoreID]    INT  NULL,
    [GotItemDate]        DATE CONSTRAINT [DEFAULT_ShoppingItemList_GotItemDate] DEFAULT (getdate()) NOT NULL,
    [NeedDate]           DATE NULL,
    [GotItem]            BIT  CONSTRAINT [DEFAULT_ShoppingItemList_GotItem] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ShoppingItemList] PRIMARY KEY CLUSTERED ([ShoppingItemListID] ASC),
    CONSTRAINT [FK_ShoppingItemList_ShoppingItems] FOREIGN KEY ([ShoppingItemListID]) REFERENCES [Shopping].[ShoppingItems] ([ShoppingItemID]),
    CONSTRAINT [FK_ShoppingItemList_ShoppingStores] FOREIGN KEY ([ShoppingItemListID]) REFERENCES [Shopping].[ShoppingStores] ([ShoppingStoreID])
);

