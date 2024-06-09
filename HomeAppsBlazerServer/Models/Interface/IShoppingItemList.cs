﻿namespace HomeAppsBlazerServer.Models.Interface
{
    public interface IShoppingItemList
    {
        // Item details
        IShoppingItem ShoppingItem { get; set; }
        int ShoppingListItemID { get; set; }
        IShoppingStore ShoppingStore { get; set; }
        bool GotItem { get; set; }
        DateTime? GotItemDate { get; set; }
        DateTime? NeedDate { get; set; }

        // Store-related details
        int? ShoppingStoreID { get; set; }
        //decimal? Price { get; set; }

        // Quantity information
        int? NumberOfItems { get; set; }

        decimal? Price { get; set; }

    }

}