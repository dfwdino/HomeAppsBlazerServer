namespace HomeAppsBlazerServer.Models.Interface
{
    public interface IShoppingItemList
    {
        // Item details
        int ShoppingItemListID { get; set; }
        int ShoppingItemID { get; set; }

        bool GotItem { get; set; }
        DateTime? GotItemDate { get; set; }
        DateTime? NeedDate { get; set; }

        // Store-related details
        int? ShoppingStoreID { get; set; }


        // Quantity information
        int? NumberOfItems { get; set; }


    }

}