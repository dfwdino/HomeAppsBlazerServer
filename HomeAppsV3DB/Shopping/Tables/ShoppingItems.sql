CREATE TABLE [Shopping].[ShoppingItems] (
    [ShoppingItemID]  INT            IDENTITY (1, 1) NOT NULL,
    [ItemName]        NVARCHAR (50)  NOT NULL,
    [IsDeleted]       BIT            CONSTRAINT [DEFAULT_ShoppingItems_IsDeleted] DEFAULT ((0)) NOT NULL,
    [IsGlutenFree]    BIT            CONSTRAINT [DEFAULT_ShoppingItems_IsGlutenFree] DEFAULT ((0)) NOT NULL,
    [KidsDontLike]    BIT            CONSTRAINT [DEFAULT_ShoppingItems_KidsDontLike] DEFAULT ((0)) NOT NULL,
    [FreddyDontLike]  BIT            CONSTRAINT [DEFAULT_ShoppingItems_FreddyDontLike] DEFAULT ((0)) NOT NULL,
    [ElliottDontLike] BIT            CONSTRAINT [DEFAULT_ShoppingItems_ElliottDontLike] DEFAULT ((0)) NOT NULL,
    [Price]           DECIMAL (5, 2) NULL,
    CONSTRAINT [PK_ShoppingItems] PRIMARY KEY CLUSTERED ([ShoppingItemID] ASC)
);



