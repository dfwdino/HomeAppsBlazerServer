namespace HomeAppsBlazerServer.Models.Interface
{
    public interface IShoppingItemResult
    {
        string ItemName { get; set; }
        int ShoppingItemListID { get; set; }
        int? NumberOfItems { get; set; }
        decimal? Price { get; set; }
    }
}