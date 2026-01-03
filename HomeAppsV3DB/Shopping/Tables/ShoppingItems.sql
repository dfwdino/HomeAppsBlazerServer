CREATE TABLE [Shopping].[ShoppingItems] (
    [ShoppingItemID]  INT           IDENTITY (1, 1) NOT NULL,
    [ItemName]        NVARCHAR (50) NOT NULL,
    [IsDeleted]       BIT           CONSTRAINT [DEFAULT_ShoppingItems_IsDeleted] DEFAULT ((0)) NOT NULL,
    [IsGlutenFree]    BIT           CONSTRAINT [DEFAULT_ShoppingItems_IsGlutenFree] DEFAULT ((0)) NOT NULL,
    [KidsDontLike]    BIT           CONSTRAINT [DEFAULT_ShoppingItems_KidsDontLike] DEFAULT ((0)) NOT NULL,
    [FreddyDontLike]  BIT           CONSTRAINT [DEFAULT_ShoppingItems_FreddyDontLike] DEFAULT ((0)) NOT NULL,
    [ElliottDontLike] BIT           CONSTRAINT [DEFAULT_ShoppingItems_ElliottDontLike] DEFAULT ((0)) NOT NULL,
    [StoreID]         INT           NULL,
    [ItemBrandsID]    INT           NULL,
    [IsOneTimeOnly]   BIT           CONSTRAINT [DF_ShoppingItems_IsOneTimeOnly] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_ShoppingItems] PRIMARY KEY CLUSTERED ([ShoppingItemID] ASC),
    CONSTRAINT [FK_ShoppingItems_ItemBrands] FOREIGN KEY ([ItemBrandsID]) REFERENCES [Shopping].[ItemBrands] ([ItemBrandsID]),
    CONSTRAINT [FK_ShoppingItems_ShoppingStores] FOREIGN KEY ([StoreID]) REFERENCES [Shopping].[ShoppingStores] ([ShoppingStoreID])
);

