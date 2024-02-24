namespace HomeAppsBlazerServer.Models.Interface
{
    public interface IShoppingList
    {
        bool GotItem { get; set; }
        DateTime GotItemDate { get; set; }
        DateTime NeedDate { get; set; }
        int ShoppingItemID { get; set; }
        int ShoppingItemListID { get; set; }
        int ShoppingStoreID { get; set; }
    }
}