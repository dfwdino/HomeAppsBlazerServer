using HomeAppsBlazerServer.Models.Shopping.Interface;

namespace HomeAppsBlazerServer.Models.Shopping
{
    public class ShoppingItemResult : IShoppingItemResult
    {
        public int ShoppingItemListID { get; set; }
        public string ItemName { get; set; }

        public int? NumberOfItems { get; set; }
        public decimal? Price { get; set; }
        public int ShoppingListID { get; set; }

        public int ItemID { get; set; }

        public string? storename { get; set; }
    }
}
