namespace HomeAppsBlazerServer.Models.Shopping.Interface
{
    public interface IShoppingItemResult
    {
        string ItemName { get; set; }
        int ShoppingItemListID { get; set; }
        int? NumberOfItems { get; set; }
        decimal? Price { get; set; }

        string? Notes { get; set; }
    }
}