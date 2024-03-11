namespace HomeAppsBlazerServer.Models.Interface
{
    public interface IShoppingItemResult
    {
        string ItemName { get; set; }
        int ShoppingItemListID { get; set; }
        ShoppingStore ShoppingStore { get; set; }
        int? NumberOfItems { get; set; }

        decimal? Price { get; set; }
    }
}