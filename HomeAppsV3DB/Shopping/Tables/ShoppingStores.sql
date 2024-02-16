CREATE TABLE [Shopping].[ShoppingStores] (
    [ShoppingStoreID] INT            IDENTITY (1, 1) NOT NULL,
    [StoreName]       NVARCHAR (50)  NOT NULL,
    [Address]         NVARCHAR (MAX) NULL,
    [IsDeleted]       BIT            CONSTRAINT [DEFAULT_ShoppingStores_IsDeleted] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_ShoppingStores] PRIMARY KEY CLUSTERED ([ShoppingStoreID] ASC)
);

