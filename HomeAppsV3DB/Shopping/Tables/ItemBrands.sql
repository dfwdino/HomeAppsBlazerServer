CREATE TABLE [Shopping].[ItemBrands] (
    [ItemBrandsID] INT           IDENTITY (1, 1) NOT NULL,
    [BrandName]    NVARCHAR (50) NOT NULL,
    [IsDeleted]    BIT           CONSTRAINT [DEFAULT_ItemBrands_IsDeleted] DEFAULT ((0)) NULL,
    CONSTRAINT [ItemBrandsID] PRIMARY KEY CLUSTERED ([ItemBrandsID] ASC)
);

